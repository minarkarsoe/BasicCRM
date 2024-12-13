using Microsoft.AspNetCore.Mvc.Rendering;
using Recsite_Ats.Application.Common.Interface.Businesslogic;
using Recsite_Ats.Application.Common.Interface.Repository;
using Recsite_Ats.Application.Common.Interface.Services;
using Recsite_Ats.Domain.API.APIRequest;
using Recsite_Ats.Domain.API.APIResponse;
using Recsite_Ats.Domain.DataTransferObject;
using Recsite_Ats.Domain.Entites;
using Recsite_Ats.Infrastructure.CustomException;
using System.Reflection;
using System.Text.Json;

namespace Recsite_Ats.Infrastructure.BusinessLogic;

public class CandidateBusinessLogic : ICandidateBusinesslogic
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IServices _services;

    public CandidateBusinessLogic(IUnitOfWork unitOfWork, IServices services)
    {
        _unitOfWork = unitOfWork;
        _services = services;
    }

    public async Task<CandidateResponseDTO> GetAllCandidates(CandidateRequestDTO requestDTO)
    {
        var candidates = await _unitOfWork.Candidate.GetAll(x => x.AccountId == requestDTO.AccountId);
        var result = await _services.SettingService.GetFieldDetailsAsync(requestDTO.TableName, requestDTO.AccountId, requestDTO.Columns);
        result.CustomFields = result.CustomFields.Where(x => x.IsVisible).ToList();
        if (result.CustomFields.Where(x => x.IsCustomField && x.CustomFieldTypeName == "MultiSelect").Any())
        {
            foreach (var options in result.CustomFields.Where(x => x.IsCustomField && x.CustomFieldTypeName == "MultiSelect"))
            {
                var viewValues = await _unitOfWork.CustomField.Get(x => x.TableName == options.TableName && x.FieldName == options.FieldName && x.AccountId == requestDTO.AccountId);
                if (viewValues != null)
                {
                    options.AvailableOptions = viewValues.ViewValues.Split(',').Select(x => new SelectListItem { Text = x, Value = x }).ToList();
                }
            }
        }
        CandidateResponseDTO responseDTO = new CandidateResponseDTO()
        {
            Candidates = candidates,
            SectionLayout = result
        };
        return responseDTO;
    }

    public async Task<SettingDataAPIResponse> GetCandidate(CandidateRequestDTO requestDTO)
    {
        var result = await _services.SettingService.GetFieldDetailsAsync(requestDTO.TableName, requestDTO.AccountId, requestDTO.Columns);
        result.CustomFields = result.CustomFields.Where(x => x.IsVisible).ToList();
        var candidate = await _unitOfWork.Candidate.Get(x => x.Id == requestDTO.CandidateId && x.AccountId == requestDTO.AccountId);
        if (candidate == null)
        {
            throw new BusinessLogicException("404", "This Candidate does not exist.");
        }
        var propertyDetails = candidate
            .GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => new
            {
                PropertyName = p.Name,
                PropertyValue = p.GetValue(candidate)
            }).ToList();

        var customFields = await _unitOfWork.CustomField.GetAll(x => x.AccountId == candidate.AccountId && x.TableName == requestDTO.TableName);
        foreach (var customField in customFields)
        {
            var customFieldDetails = result.CustomFields.FirstOrDefault(x => x.FieldName == customField.FieldName);
            if (customFieldDetails != null)
            {
                var storeCustomFieldValue = await _unitOfWork.CustomFieldStoreValue.Get(x => x.CustomFieldId == customField.Id && x.TableId == candidate.Id);
                if (storeCustomFieldValue != null)
                {
                    if (customFieldDetails.CustomFieldTypeName == "Image")
                    {
                        var base64Sting = await _services.S3Service.GetBase64ImageStringAsync(storeCustomFieldValue.StoreValue);
                        customFieldDetails.FieldValue = storeCustomFieldValue.StoreValue;
                        customFieldDetails.Base64String = base64Sting;
                    }
                    else if (customFieldDetails.CustomFieldTypeName == "DropDown" || customFieldDetails.CustomFieldTypeName == "MultiSelect")
                    {
                        customFieldDetails.SelectedOptions = customField.ViewValues?.Split(',').ToList();
                        customFieldDetails.FieldValue = storeCustomFieldValue.StoreValue;
                    }
                    else
                    {
                        customFieldDetails.FieldValue = storeCustomFieldValue.StoreValue;
                    }
                }

            }
        }
        foreach (var propertyDetail in propertyDetails)
        {
            var customField = result.CustomFields.FirstOrDefault(x => x.FieldName == propertyDetail.PropertyName);
            if (customField != null)
            {
                if (customField.CustomFieldTypeName == "Image")
                {
                    var base64string = await _services.S3Service.GetBase64ImageStringAsync(propertyDetail.PropertyValue?.ToString() ?? string.Empty);
                    customField.FieldValue = propertyDetail.PropertyValue?.ToString();
                    customField.Base64String = base64string;
                }
                else
                {
                    customField.FieldValue = propertyDetail.PropertyValue?.ToString() ?? string.Empty;
                }
            }
        }
        result.CustomFields = result.CustomFields.Where(x => x.IsVisible).ToList();
        SettingDataAPIResponse responseDTO = new SettingDataAPIResponse()
        {
            CustomFields = result.CustomFields,
            Sections = result.Sections,
        };

        return responseDTO;
    }

    public async Task CreateCandidate(SettingCreateRequest requestDTO, int? accountId)
    {
        foreach (var image in requestDTO.DataFields.Where(x => x.CustomFieldTypeName == "Image"))
        {

            var imageUpload = JsonSerializer.Deserialize<S3ImageUploadRequest>(image.FieldValue);
            imageUpload.Key = "logo";
            var result = await _services.S3Service.UploadS3File(imageUpload);
            image.FieldValue = result.FilePath;
        }

        // Retrieve all non-custom fields (default fields)
        var defaultFields = requestDTO.DataFields.Where(x => !x.IsCustomField).ToList();

        // Create a new candidate object
        var candidate = new Candidate
        {
            AccountId = accountId.Value,
            FirstName = defaultFields.FirstOrDefault(x => x.FieldName == nameof(Candidate.FirstName))?.FieldValue,
            LastName = defaultFields.FirstOrDefault(x => x.FieldName == nameof(Candidate.LastName))?.FieldValue,
            Email = defaultFields.FirstOrDefault(x => x.FieldName == nameof(Candidate.Email))?.FieldValue,
            Phone = defaultFields.FirstOrDefault(x => x.FieldName == nameof(Candidate.Phone))?.FieldValue,
            CreatedDate = DateTime.Now,
            CreatedBy = accountId.Value
        };

        // Add the company to the database
        await _unitOfWork.Candidate.Add(candidate);
        await _unitOfWork.Save();

        // Handle custom fields
        var customFields = requestDTO.DataFields.Where(x => x.IsCustomField).ToList();
        foreach (var customField in customFields)
        {
            // Retrieve the custom field definition
            var existingCustomField = await _unitOfWork.CustomField.Get(x => x.FieldName == customField.FieldName && x.AccountId == accountId);

            if (existingCustomField != null)
            {
                // Create a new StoreCustomFieldValue entity
                var storeCustomField = new StoreCustomFieldValue
                {
                    CustomFieldId = existingCustomField.Id,
                    TableName = existingCustomField.TableName,
                    TableId = candidate.Id,
                    StoreValue = customField.FieldValue
                };

                // Add the store custom field value to the database
                await _unitOfWork.CustomFieldStoreValue.Add(storeCustomField);
                // Save changes for custom fields
            }
        }
        await _unitOfWork.Save();
    }

    public async Task EditCandidate(SettingCreateRequest requestDTO, CandidateRequestDTO candidateData)
    {
        var candidate = await _unitOfWork.Candidate.Get(x => x.Id == candidateData.CandidateId && x.AccountId == candidateData.AccountId);
        if (candidate == null)
        {
            new BusinessLogicException("404", "Data is not founded!");
        }
        foreach (var image in requestDTO.DataFields.Where(x => x.CustomFieldTypeName == "Image"))
        {
            if (image.ImageData != null)
            {
                image.ImageData.Key = "logo";
                var result = await _services.S3Service.UploadS3File(image.ImageData);
                image.FieldValue = result.FilePath;
                image.Base64String = image.ImageData.Base64ImageData;
            }
        }
        var defaultFieldlist = requestDTO.DataFields.Where(x => !x.IsCustomField).ToList();

        candidate.AccountId = candidateData.AccountId.Value;
        candidate.FirstName = defaultFieldlist.Where(x => x.FieldName == nameof(Candidate.FirstName)).Select(x => x.FieldValue).FirstOrDefault() ?? candidate.FirstName;
        candidate.LastName = defaultFieldlist.Where(x => x.FieldName == nameof(Candidate.LastName)).Select(x => x.FieldValue).FirstOrDefault() ?? candidate.LastName;
        candidate.Email = defaultFieldlist.Where(x => x.FieldName == nameof(Candidate.Email)).Select(x => x.FieldValue).FirstOrDefault() ?? candidate.Email;
        candidate.Phone = defaultFieldlist.Where(x => x.FieldName == nameof(Candidate.Phone)).Select(x => x.FieldValue).FirstOrDefault() ?? candidate.Phone;
        candidate.EditedDate = DateTime.Now;
        candidate.EditedBy = candidateData.AccountId.Value;
        await _unitOfWork.Candidate.Update(candidate);

        var customFields = requestDTO.DataFields.Where(x => x.IsCustomField).ToList();
        foreach (var customField in customFields)
        {
            var getCustomField = await _unitOfWork.CustomField.Get(x => x.FieldName == customField.FieldName && x.AccountId == candidateData.AccountId && x.TableName == customField.TableName);
            if (getCustomField != null)
            {
                var storeCustomFieldValue = await _unitOfWork.CustomFieldStoreValue.Get(x => x.CustomFieldId == getCustomField.Id && x.TableId == candidate.Id);
                if (storeCustomFieldValue != null)
                {
                    storeCustomFieldValue.StoreValue = customField.FieldValue;
                    await _unitOfWork.CustomFieldStoreValue.Update(storeCustomFieldValue);
                }

            }
        }
        await _unitOfWork.Save();
    }

    public async Task DeleteCandidate(CompanyDataRequestDTO requestDTO)
    {
        var result = await _services.SettingService.GetFieldDetailsAsync(requestDTO.TableName, requestDTO.AccountId, requestDTO.Columns);
        result.CustomFields = result.CustomFields.Where(x => x.IsVisible).ToList();
        var company = await _unitOfWork.Company.Get(x => x.Id == requestDTO.CompanyId && x.AccountId == requestDTO.AccountId);
        if (company != null)
        {
            _unitOfWork.Company.Remove(company);
        }

        if (!string.IsNullOrEmpty(company.Logo))
        {
            await _services.S3Service.DeleteFileAsync(company.Logo);
        }

        var customFields = await _unitOfWork.CustomField.GetAll(x => x.AccountId == requestDTO.AccountId && x.TableName == "Companies");
        foreach (var customField in customFields)
        {
            var customFieldDetails = result.CustomFields.FirstOrDefault(x => x.FieldName == customField.FieldName);
            if (customFieldDetails != null)
            {
                var storeCustomFieldValue = await _unitOfWork.CustomFieldStoreValue.Get(x => x.CustomFieldId == customField.Id && x.TableId == requestDTO.CompanyId);
                if (storeCustomFieldValue != null)
                {
                    if (customFieldDetails.CustomFieldTypeName == "Image")
                    {
                        await _services.S3Service.DeleteFileAsync(storeCustomFieldValue.StoreValue);
                    }
                    _unitOfWork.CustomFieldStoreValue.Remove(storeCustomFieldValue);
                }
            }
        }
        await _unitOfWork.Save();
    }

    public async Task<List<SearchDataResponse>> SearchResults(int accountId)
    {
        var candidates = await _unitOfWork.Candidate.GetAll(x => x.AccountId == accountId);

        var searchResults = new List<SearchDataResponse>();
        foreach (var candidate in candidates)
        {
            /*SearchDataResponse searchDataResponse = new SearchDataResponse();
            searchDataResponse.Id = candidate.Id;
            searchDataResponse.Name = $"{candidate.FirstName} {candidate.LastName}";
            searchResults.Add(searchDataResponse);*/
        }

        return searchResults;
    }
}


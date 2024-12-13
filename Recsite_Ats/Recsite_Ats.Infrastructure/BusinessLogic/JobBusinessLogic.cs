using Recsite_Ats.Application.Common.Helpers;
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
public class JobBusinessLogic : IJobBusinessLogic
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IServices _services;

    public JobBusinessLogic(IUnitOfWork unitOfWork, IServices services)
    {
        _unitOfWork = unitOfWork;
        _services = services;
    }

    public async Task<JobResponseDTO> GetAllJobs(JobRequestDTO requestDTO)
    {
        var jobs = await _unitOfWork.Job.GetAll(x => x.AccountId == requestDTO.AccountId);
        var result = await _services.SettingService.GetFieldDetailsAsync(requestDTO.TableName, requestDTO.AccountId, requestDTO.Columns);
        result.CustomFields = result.CustomFields.Where(x => x.IsVisible).ToList();
        if (result.CustomFields.Where(x => x.IsCustomField && (x.CustomFieldTypeName == "MultiSelect" || x.CustomFieldTypeName == "DropDown")).Any())
        {
            foreach (var options in result.CustomFields.Where(x => x.IsCustomField && (x.CustomFieldTypeName == "MultiSelect" || x.CustomFieldTypeName == "DropDown")))
            {
                var viewValues = await _unitOfWork.CustomField.Get(x => x.TableName == options.TableName && x.FieldName == options.FieldName && x.AccountId == requestDTO.AccountId);
                if (viewValues != null)
                {
                    options.SelectedOptions = viewValues.ViewValues?.Split(',').ToList();
                }
            }
        }
        JobResponseDTO responseDTO = new JobResponseDTO()
        {
            Jobs = jobs,
            SectionLayout = result
        };
        return responseDTO;
    }

    public async Task<SettingDataAPIResponse> GetJob(JobRequestDTO requestDTO)
    {
        var result = await _services.SettingService.GetFieldDetailsAsync(requestDTO.TableName, requestDTO.AccountId, requestDTO.Columns);
        result.CustomFields = result.CustomFields.Where(x => x.IsVisible).ToList();
        var job = await _unitOfWork.Job.Get(x => x.Id == requestDTO.JobId && x.AccountId == requestDTO.AccountId);
        if (job == null)
        {
            throw new BusinessLogicException("404", "This job does not exist.");
        }
        var propertyDetails = job
            .GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => new
            {
                PropertyName = p.Name,
                PropertyValue = p.GetValue(job)
            }).ToList();

        var customFields = await _unitOfWork.CustomField.GetAll(x => x.AccountId == job.AccountId && x.TableName == requestDTO.TableName);
        foreach (var customField in customFields)
        {
            var customFieldDetails = result.CustomFields.FirstOrDefault(x => x.FieldName == customField.FieldName);
            if (customFieldDetails != null)
            {
                var storeCustomFieldValue = await _unitOfWork.CustomFieldStoreValue.Get(x => x.CustomFieldId == customField.Id && x.TableId == job.Id);
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

    public async Task CreateJob(SettingCreateRequest requestDTO, int? accountId)
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

        // Create a new company object
        var job = new Job
        {
            AccountId = accountId.Value,
            Title = defaultFields.FirstOrDefault(x => x.FieldName == nameof(Job.Title))?.FieldValue,
            Summary = defaultFields.FirstOrDefault(x => x.FieldName == nameof(Job.Summary))?.FieldValue,
            WorkingHour = defaultFields.FirstOrDefault(x => x.FieldName == nameof(Job.WorkingHour))?.FieldValue,
            SalaryCurrency = defaultFields.FirstOrDefault(x => x.FieldName == nameof(Job.SalaryCurrency))?.FieldValue,
            Benefits = defaultFields.FirstOrDefault(x => x.FieldName == nameof(Job.Benefits))?.FieldValue,
            SalaryMin = string.IsNullOrEmpty(defaultFields.FirstOrDefault(x => x.FieldName == nameof(Job.SalaryMin))?.FieldValue) ? 0 : Decimal.Parse(defaultFields.FirstOrDefault(x => x.FieldName == nameof(Job.SalaryMin)).FieldValue),
            SalaryMax = string.IsNullOrEmpty(defaultFields.FirstOrDefault(x => x.FieldName == nameof(Job.SalaryMax))?.FieldValue) ? 0 : Decimal.Parse(defaultFields.FirstOrDefault(x => x.FieldName == nameof(Job.SalaryMax)).FieldValue),
            PublicSalary = defaultFields.FirstOrDefault(x => x.FieldName == nameof(Job.PublicSalary))?.FieldValue,
            StartDate = Helper.IsDateTime(defaultFields.FirstOrDefault(x => x.FieldName == nameof(Job.StartDate))?.FieldValue) ? DateTime.Parse(defaultFields.FirstOrDefault(x => x.FieldName == nameof(Job.StartDate))?.FieldValue) : null,
            CategoryId = Helper.IsNumber(defaultFields.FirstOrDefault(x => x.FieldName == nameof(Job.CategoryId))?.FieldValue) ? int.Parse(defaultFields.FirstOrDefault(x => x.FieldName == nameof(Job.CategoryId))?.FieldValue) : 0,
            LocationId = Helper.IsNumber(defaultFields.FirstOrDefault(x => x.FieldName == nameof(Job.LocationId))?.FieldValue) ? int.Parse(defaultFields.FirstOrDefault(x => x.FieldName == nameof(Job.LocationId))?.FieldValue) : 0,
            EmploymentTypeId = Helper.IsNumber(defaultFields.FirstOrDefault(x => x.FieldName == nameof(Job.EmploymentTypeId))?.FieldValue) ? int.Parse(defaultFields.FirstOrDefault(x => x.FieldName == nameof(Job.EmploymentTypeId))?.FieldValue) : 0,
            City = defaultFields.FirstOrDefault(x => x.FieldName == nameof(Job.City))?.FieldValue,
            CountryCode = defaultFields.FirstOrDefault(x => x.FieldName == nameof(Job.CountryCode))?.FieldValue,
            ExperienceRequirements = defaultFields.FirstOrDefault(x => x.FieldName == nameof(Job.ExperienceRequirements))?.FieldValue,
            ExpiryDate = Helper.IsDateTime(defaultFields.FirstOrDefault(x => x.FieldName == nameof(Job.ExpiryDate))?.FieldValue) ? DateTime.Parse(defaultFields.FirstOrDefault(x => x.FieldName == nameof(Job.ExpiryDate))?.FieldValue) : null,
            IncentiveCompensation = defaultFields.FirstOrDefault(x => x.FieldName == nameof(Job.IncentiveCompensation))?.FieldValue,
            IsPublic = Helper.IsBoolean(defaultFields.FirstOrDefault(x => x.FieldName == nameof(Job.IsPublic))?.FieldValue) ? Boolean.Parse((defaultFields.FirstOrDefault(x => x.FieldName == nameof(Job.IsPublic))?.FieldValue)) : false,
            JobBenefits = defaultFields.FirstOrDefault(x => x.FieldName == nameof(Job.JobBenefits))?.FieldValue,
            JobOwnerUserId = Helper.IsNumber(defaultFields.FirstOrDefault(x => x.FieldName == nameof(Job.JobOwnerUserId))?.FieldValue) ? int.Parse((defaultFields.FirstOrDefault(x => x.FieldName == nameof(Job.JobOwnerUserId))?.FieldValue)) : 0,
            Latitude = defaultFields.FirstOrDefault(x => x.FieldName == nameof(Job.Latitude))?.FieldValue,
            Longitude = defaultFields.FirstOrDefault(x => x?.FieldName == nameof(Job.Longitude))?.FieldValue,
            PostCode = defaultFields.FirstOrDefault(x => x?.FieldName == nameof(Job.PostCode))?.FieldValue,
            PublicDescription = defaultFields.FirstOrDefault(x => x?.FieldName == nameof(Job.PublicDescription))?.FieldValue,
            Qualifications = defaultFields.FirstOrDefault(x => x?.FieldName == nameof(Job.Qualifications))?.FieldValue,
            Responsibilities = defaultFields.FirstOrDefault(x => x?.FieldName == nameof(Job.Responsibilities))?.FieldValue,
            SalaryUnit = defaultFields.FirstOrDefault(x => x?.FieldName == nameof(Job.SalaryUnit))?.FieldValue,
            Skills = defaultFields.FirstOrDefault(x => x?.FieldName == nameof(Job.Skills))?.FieldValue,
            State = defaultFields.FirstOrDefault(x => x?.FieldName == nameof(Job.State))?.FieldValue,
            StreetAddress = defaultFields.FirstOrDefault(x => x?.FieldName == nameof(Job.StreetAddress))?.FieldValue,
            CreatedDate = DateTime.Now,
            CreatedBy = accountId.Value,
            EditedDate = DateTime.Now,
        };

        // Add the company to the database
        await _unitOfWork.Job.Add(job);
        await _unitOfWork.Save();

        // Handle custom fields
        var customFields = requestDTO.DataFields.Where(x => x.IsCustomField).ToList();
        foreach (var customField in customFields)
        {
            if (customField.FieldValue == null) continue;

            // Retrieve the custom field definition
            var existingCustomField = await _unitOfWork.CustomField.Get(x => x.FieldName == customField.FieldName && x.AccountId == accountId);

            if (existingCustomField != null)
            {
                // Create a new StoreCustomFieldValue entity
                var storeCustomField = new StoreCustomFieldValue
                {
                    CustomFieldId = existingCustomField.Id,
                    TableName = existingCustomField.TableName,
                    TableId = job.Id,
                    StoreValue = customField.FieldValue
                };

                // Add the store custom field value to the database
                await _unitOfWork.CustomFieldStoreValue.Add(storeCustomField);
                // Save changes for custom fields
            }
        }
        await _unitOfWork.Save();
    }

    public async Task EditJob(SettingCreateRequest requestDTO, JobRequestDTO jobData)
    {
        var job = await _unitOfWork.Job.Get(x => x.Id == jobData.JobId && x.AccountId == jobData.AccountId);
        if (job == null)
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
        job.AccountId = jobData.AccountId.Value;
        job.Title = defaultFieldlist.FirstOrDefault(x => x.FieldName == nameof(Job.Title))?.FieldValue;
        job.Summary = defaultFieldlist.FirstOrDefault(x => x.FieldName == nameof(Job.Summary))?.FieldValue;
        job.WorkingHour = defaultFieldlist.FirstOrDefault(x => x.FieldName == nameof(Job.WorkingHour))?.FieldValue;
        job.SalaryCurrency = defaultFieldlist.FirstOrDefault(x => x.FieldName == nameof(Job.SalaryCurrency))?.FieldValue;
        job.Benefits = defaultFieldlist.FirstOrDefault(x => x.FieldName == nameof(Job.Benefits))?.FieldValue;
        job.SalaryMin = string.IsNullOrEmpty(defaultFieldlist.FirstOrDefault(x => x.FieldName == nameof(Job.SalaryMin))?.FieldValue) ? 0 : Decimal.Parse(defaultFieldlist.FirstOrDefault(x => x.FieldName == nameof(Job.SalaryMin)).FieldValue);
        job.SalaryMax = string.IsNullOrEmpty(defaultFieldlist.FirstOrDefault(x => x.FieldName == nameof(Job.SalaryMax))?.FieldValue) ? 0 : Decimal.Parse(defaultFieldlist.FirstOrDefault(x => x.FieldName == nameof(Job.SalaryMax)).FieldValue);
        job.PublicSalary = defaultFieldlist.FirstOrDefault(x => x.FieldName == nameof(Job.PublicSalary))?.FieldValue;
        job.StartDate = Helper.IsDateTime(defaultFieldlist.FirstOrDefault(x => x.FieldName == nameof(Job.StartDate))?.FieldValue) ? DateTime.Parse(defaultFieldlist.FirstOrDefault(x => x.FieldName == nameof(Job.StartDate))?.FieldValue) : null;
        job.CategoryId = Helper.IsNumber(defaultFieldlist.FirstOrDefault(x => x.FieldName == nameof(Job.CategoryId))?.FieldValue) ? int.Parse(defaultFieldlist.FirstOrDefault(x => x.FieldName == nameof(Job.CategoryId))?.FieldValue) : 0;
        job.LocationId = Helper.IsNumber(defaultFieldlist.FirstOrDefault(x => x.FieldName == nameof(Job.LocationId))?.FieldValue) ? int.Parse(defaultFieldlist.FirstOrDefault(x => x.FieldName == nameof(Job.LocationId))?.FieldValue) : 0;
        job.EmploymentTypeId = Helper.IsNumber(defaultFieldlist.FirstOrDefault(x => x.FieldName == nameof(Job.EmploymentTypeId))?.FieldValue) ? int.Parse(defaultFieldlist.FirstOrDefault(x => x.FieldName == nameof(Job.EmploymentTypeId))?.FieldValue) : 0;
        job.City = defaultFieldlist.FirstOrDefault(x => x.FieldName == nameof(Job.City))?.FieldValue;
        job.CountryCode = defaultFieldlist.FirstOrDefault(x => x.FieldName == nameof(Job.CountryCode))?.FieldValue;
        job.ExperienceRequirements = defaultFieldlist.FirstOrDefault(x => x.FieldName == nameof(Job.ExperienceRequirements))?.FieldValue;
        job.ExpiryDate = Helper.IsDateTime(defaultFieldlist.FirstOrDefault(x => x.FieldName == nameof(Job.ExpiryDate))?.FieldValue) ? DateTime.Parse(defaultFieldlist.FirstOrDefault(x => x.FieldName == nameof(Job.ExpiryDate))?.FieldValue) : null;
        job.IncentiveCompensation = defaultFieldlist.FirstOrDefault(x => x.FieldName == nameof(Job.IncentiveCompensation))?.FieldValue;
        job.IsPublic = Helper.IsBoolean(defaultFieldlist.FirstOrDefault(x => x.FieldName == nameof(Job.IsPublic))?.FieldValue) ? Boolean.Parse((defaultFieldlist.FirstOrDefault(x => x.FieldName == nameof(Job.IsPublic))?.FieldValue)) : false;
        job.JobBenefits = defaultFieldlist.FirstOrDefault(x => x.FieldName == nameof(Job.JobBenefits))?.FieldValue;
        job.JobOwnerUserId = Helper.IsNumber(defaultFieldlist.FirstOrDefault(x => x.FieldName == nameof(Job.JobOwnerUserId))?.FieldValue) ? int.Parse((defaultFieldlist.FirstOrDefault(x => x.FieldName == nameof(Job.JobOwnerUserId))?.FieldValue)) : 0;
        job.Latitude = defaultFieldlist.FirstOrDefault(x => x.FieldName == nameof(Job.Latitude))?.FieldValue;
        job.Longitude = defaultFieldlist.FirstOrDefault(x => x?.FieldName == nameof(Job.Longitude))?.FieldValue;
        job.PostCode = defaultFieldlist.FirstOrDefault(x => x?.FieldName == nameof(Job.PostCode))?.FieldValue;
        job.PublicDescription = defaultFieldlist.FirstOrDefault(x => x?.FieldName == nameof(Job.PublicDescription))?.FieldValue;
        job.Qualifications = defaultFieldlist.FirstOrDefault(x => x?.FieldName == nameof(Job.Qualifications))?.FieldValue;
        job.Responsibilities = defaultFieldlist.FirstOrDefault(x => x?.FieldName == nameof(Job.Responsibilities))?.FieldValue;
        job.SalaryUnit = defaultFieldlist.FirstOrDefault(x => x?.FieldName == nameof(Job.SalaryUnit))?.FieldValue;
        job.Skills = defaultFieldlist.FirstOrDefault(x => x?.FieldName == nameof(Job.Skills))?.FieldValue;
        job.State = defaultFieldlist.FirstOrDefault(x => x?.FieldName == nameof(Job.State))?.FieldValue;
        job.StreetAddress = defaultFieldlist.FirstOrDefault(x => x?.FieldName == nameof(Job.StreetAddress))?.FieldValue;
        job.EditedDate = DateTime.Now;
        job.EditedBy = jobData.AccountId.Value;
        await _unitOfWork.Job.Update(job);

        var customFields = requestDTO.DataFields.Where(x => x.IsCustomField).ToList();
        foreach (var customField in customFields)
        {
            var getCustomField = await _unitOfWork.CustomField.Get(x => x.FieldName == customField.FieldName && x.AccountId == job.AccountId && x.TableName == customField.TableName);
            if (getCustomField != null)
            {
                var storeCustomFieldValue = await _unitOfWork.CustomFieldStoreValue.Get(x => x.CustomFieldId == getCustomField.Id && x.TableId == job.Id);
                if (storeCustomFieldValue != null)
                {
                    storeCustomFieldValue.StoreValue = customField.FieldValue;
                    await _unitOfWork.CustomFieldStoreValue.Update(storeCustomFieldValue);
                }

            }
        }
        await _unitOfWork.Save();
    }

    public async Task DeleteJob(CompanyDataRequestDTO requestDTO)
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
}

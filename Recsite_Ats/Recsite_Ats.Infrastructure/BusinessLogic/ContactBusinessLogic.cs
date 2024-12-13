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
public class ContactBusinessLogic : IContactBusinessLogic
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IServices _services;
    private readonly IEmailSender _emailSender;

    public ContactBusinessLogic(IUnitOfWork unitOfWork, IServices services, IEmailSender emailSender)
    {
        _unitOfWork = unitOfWork;
        _services = services;
        _emailSender = emailSender;
    }

    public async Task<ContactDataResponseDTO> GetAllContacts(ContactDataRequestDTO requestDTO)
    {
        var contacts = await _unitOfWork.Contact.GetAll(x => x.AccountId == requestDTO.AccountId);
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
        ContactDataResponseDTO responseDTO = new ContactDataResponseDTO()
        {
            Contacts = contacts,
            SectionLayout = result
        };
        return responseDTO;
    }

    public async Task<SettingDataAPIResponse> GetContact(ContactDataRequestDTO requestDTO)
    {
        var result = await _services.SettingService.GetFieldDetailsAsync(requestDTO.TableName, requestDTO.AccountId, requestDTO.Columns);
        result.CustomFields = result.CustomFields.Where(x => x.IsVisible).ToList();
        var contact = await _unitOfWork.Contact.Get(x => x.Id == requestDTO.ContactId && x.AccountId == requestDTO.AccountId);
        if (contact == null)
        {
            throw new BusinessLogicException("404", "This contact does not exist.");
        }
        var propertyDetails = contact
            .GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => new
            {
                PropertyName = p.Name,
                PropertyValue = p.GetValue(contact)
            }).ToList();

        var customFields = await _unitOfWork.CustomField.GetAll(x => x.AccountId == contact.AccountId && x.TableName == requestDTO.TableName);
        foreach (var customField in customFields)
        {
            var customFieldDetails = result.CustomFields.FirstOrDefault(x => x.FieldName == customField.FieldName);
            if (customFieldDetails != null)
            {
                var storeCustomFieldValue = await _unitOfWork.CustomFieldStoreValue.Get(x => x.CustomFieldId == customField.Id && x.TableId == contact.Id);
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

    public async Task CreateContact(SettingCreateRequest requestDTO, int? accountId)
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
        var contact = new Contact
        {
            AccountId = accountId.Value,
            FirstName = defaultFields.FirstOrDefault(x => x.FieldName == nameof(Contact.FirstName))?.FieldValue,
            LastName = defaultFields.FirstOrDefault(x => x.FieldName == nameof(Contact.LastName))?.FieldValue,
            Phone = defaultFields.FirstOrDefault(x => x.FieldName == nameof(Contact.Phone))?.FieldValue,
            Email = defaultFields.FirstOrDefault(x => x.FieldName == nameof(Contact.Phone))?.FieldValue,
            LinkedInUrl = defaultFields.FirstOrDefault(x => x.FieldName == nameof(Contact.LinkedInUrl))?.FieldValue,
            CreatedDate = DateTime.Now,
            CreatedBy = accountId.Value
        };

        // Add the company to the database
        await _unitOfWork.Contact.Add(contact);
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
                    TableId = contact.Id,
                    StoreValue = customField.FieldValue
                };

                // Add the store custom field value to the database
                await _unitOfWork.CustomFieldStoreValue.Add(storeCustomField);
                // Save changes for custom fields
            }
        }
        await _unitOfWork.Save();

        var getEmailTemplate = await _unitOfWork.EmailTemplate.Get(x => x.ModuleId == 1 && x.IsDefault && (x.AccountId == accountId || x.AccountId == 0));
        var account = await _unitOfWork.Account.Get(x => x.Id == accountId);
        await _emailSender.SendMessage(account.ContactEmail, getEmailTemplate.Name, getEmailTemplate.Template);
    }

    public async Task EditContact(SettingCreateRequest requestDTO, ContactDataRequestDTO contactData)
    {
        var contact = await _unitOfWork.Contact.Get(x => x.Id == contactData.ContactId && x.AccountId == contactData.AccountId);
        if (contact == null)
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

        contact.AccountId = contactData.AccountId.Value;
        contact.FirstName = defaultFieldlist.FirstOrDefault(x => x.FieldName == nameof(Contact.FirstName))?.FieldValue;
        contact.LastName = defaultFieldlist.FirstOrDefault(x => x.FieldName == nameof(Contact.LastName))?.FieldValue;
        contact.Phone = defaultFieldlist.FirstOrDefault(x => x.FieldName == nameof(Contact.Phone))?.FieldValue;
        contact.Email = defaultFieldlist.FirstOrDefault(x => x.FieldName == nameof(Contact.Phone))?.FieldValue;
        contact.LinkedInUrl = defaultFieldlist.FirstOrDefault(x => x.FieldName == nameof(Contact.LinkedInUrl))?.FieldValue;
        contact.EditedDate = DateTime.Now;
        contact.EditedBy = contactData.AccountId.Value;
        await _unitOfWork.Contact.Update(contact);

        var customFields = requestDTO.DataFields.Where(x => x.IsCustomField).ToList();
        foreach (var customField in customFields)
        {
            var getCustomField = await _unitOfWork.CustomField.Get(x => x.FieldName == customField.FieldName && x.AccountId == contactData.AccountId && x.TableName == customField.TableName);
            if (getCustomField != null)
            {
                var storeCustomFieldValue = await _unitOfWork.CustomFieldStoreValue.Get(x => x.CustomFieldId == getCustomField.Id && x.TableId == contact.Id);
                if (storeCustomFieldValue != null)
                {
                    storeCustomFieldValue.StoreValue = customField.FieldValue;
                    await _unitOfWork.CustomFieldStoreValue.Update(storeCustomFieldValue);
                }

            }
        }
        await _unitOfWork.Save();
    }

    public async Task DeleteContact(ContactDataRequestDTO requestDTO)
    {
        var result = await _services.SettingService.GetFieldDetailsAsync(requestDTO.TableName, requestDTO.AccountId, requestDTO.Columns);
        result.CustomFields = result.CustomFields.Where(x => x.IsVisible).ToList();
        var contact = await _unitOfWork.Contact.Get(x => x.Id == requestDTO.ContactId && x.AccountId == requestDTO.AccountId);
        if (contact != null)
        {
            _unitOfWork.Contact.Remove(contact);
        }

        var customFields = await _unitOfWork.CustomField.GetAll(x => x.AccountId == requestDTO.AccountId && x.TableName == "Contacts");
        foreach (var customField in customFields)
        {
            var customFieldDetails = result.CustomFields.FirstOrDefault(x => x.FieldName == customField.FieldName);
            if (customFieldDetails != null)
            {
                var storeCustomFieldValue = await _unitOfWork.CustomFieldStoreValue.Get(x => x.CustomFieldId == customField.Id && x.TableId == requestDTO.ContactId);
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

    public async Task<SearchDataResponse> SearchResults(int accountId)
    {
        var contacts = await _unitOfWork.Contact.GetAll(x => x.AccountId == accountId);

        var searchResults = new List<SearchData>();
        foreach (var contact in contacts)
        {
            SearchData searchData = new SearchData()
            {
                Id = contact.Id,
                Name = $"{contact.FirstName} {contact.LastName}"
            };
            searchResults.Add(searchData);
        }
        var response = new SearchDataResponse()
        {
            SearchData = searchResults
        };

        return response;
    }
}

using Recsite_Ats.Application.Common.Interface.Businesslogic;
using Recsite_Ats.Application.Common.Interface.Repository;
using Recsite_Ats.Application.Common.Interface.Services;
using Recsite_Ats.Domain.API.APIRequest;
using Recsite_Ats.Domain.API.APIResponse;
using Recsite_Ats.Domain.DataTransferObject;
using Recsite_Ats.Domain.DataTransferObject.UserDTO;
using Recsite_Ats.Domain.Entites;
using Recsite_Ats.Infrastructure.CustomException;
using System.Reflection;
using System.Text.Json;

namespace Recsite_Ats.Infrastructure.BusinessLogic;
public class CompanyBusinessLogic : ICompanyBusinessLogic
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IServices _services;

    public CompanyBusinessLogic(IUnitOfWork unitOfWork, IServices services)
    {
        _unitOfWork = unitOfWork;
        _services = services;
    }

    public async Task<CompanyDataResponseDTO> GetAllCompanies(CompanyDataRequestDTO requestDTO)
    {
        var companies = await _unitOfWork.Company.GetAll(x => x.AccountId == requestDTO.AccountId);
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
        CompanyDataResponseDTO responseDTO = new CompanyDataResponseDTO()
        {
            Companies = companies,
            SectionLayout = result
        };
        return responseDTO;
    }

    public async Task<CompanyDetailDataResponse> GetCompany(CompanyDataRequestDTO requestDTO)
    {
        var result = await _services.SettingService.GetFieldDetailsAsync(requestDTO.TableName, requestDTO.AccountId, requestDTO.Columns);
        result.CustomFields = result.CustomFields.Where(x => x.IsVisible).ToList();

        var company = await _unitOfWork.Company.Get(x => x.Id == requestDTO.CompanyId && x.AccountId == requestDTO.AccountId);
        if (company == null)
        {
            throw new BusinessLogicException("404", "This company does not exist.");
        }
        var propertyDetails = company
            .GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => new
            {
                PropertyName = p.Name,
                PropertyValue = p.GetValue(company)
            }).ToList();

        var customFields = await _unitOfWork.CustomField.GetAll(x => x.AccountId == company.AccountId && x.TableName == requestDTO.TableName);
        foreach (var customField in customFields)
        {
            var customFieldDetails = result.CustomFields.FirstOrDefault(x => x.FieldName == customField.FieldName);
            if (customFieldDetails != null)
            {
                var storeCustomFieldValue = await _unitOfWork.CustomFieldStoreValue.Get(x => x.CustomFieldId == customField.Id && x.TableId == company.Id);
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
        var contactIdList = result.CustomFields.Where(x => x.FieldName == "PrimaryContact").FirstOrDefault()?.FieldValue?.Split(',').ToList();
        var companyDetails = new CompanyDetailDTO();
        companyDetails.CompanyName = company.CompanyName;
        companyDetails.Logo = result.CustomFields.Where(x => x.FieldName == "Logo").FirstOrDefault()?.Base64String;
        companyDetails.CompanyId = company.Id;
        companyDetails.LastUpdated = company.EditedDate;
        companyDetails.Contacts = new List<ContactDetailDTO>();

        if (contactIdList != null)
        {
            var contactsList = await _unitOfWork.CompanyContact.GetAll(x => x.CompanyId == company.Id, "Contact");
            foreach (var contact in contactsList)
            {
                var contactDetail = new ContactDetailDTO();
                contactDetail.ContactName = $"{contact.Contact.FirstName} {contact.Contact.LastName}";
                contactDetail.ContactId = contact.ContactId;
                contactDetail.MobileNumber = contact.Contact.Mobile;
                companyDetails.Contacts.Add(contactDetail);
            }
        }

        result.CustomFields = result.CustomFields.Where(x => x.IsVisible).ToList();

        var owner = await _unitOfWork.User.Get(x => x.Id == company.CreatedBy);
        var followers = await _unitOfWork.Followers.GetAll(x => x.CompanyId == company.Id, "User");

        List<CompanyFollwersDTO> companyFollowers = new List<CompanyFollwersDTO> {
            new CompanyFollwersDTO
            {
                IsOwner = true,
                Name = owner.FullName,
                UserId = owner.Id,
            }
        };

        foreach (var follower in followers)
        {
            var companyFollwer = new CompanyFollwersDTO()
            {
                UserId = follower.UserId,
                Name = follower.User.FullName
            };
            companyFollowers.Add(companyFollwer);
        };

        CompanyDetailDataResponse responseDTO = new CompanyDetailDataResponse()
        {
            CustomFields = result.CustomFields,
            Sections = result.Sections,
            CompanyDetails = companyDetails,
            CompanyFollowers = companyFollowers
        };

        return responseDTO;
    }

    public async Task CreateCompany(SettingCreateRequest requestDTO, ClaimTypesDto claimTypes)
    {
        foreach (var image in requestDTO.DataFields.Where(x => x.CustomFieldTypeName == "Image"))
        {
            image.ImageData.Key = "logo";
            var result = await _services.S3Service.UploadS3File(image.ImageData);
            image.FieldValue = result.FilePath;
        }

        // Retrieve all non-custom fields (default fields)
        var defaultFields = requestDTO.DataFields.Where(x => !x.IsCustomField).ToList();


        // Create a new company object
        var company = new Company
        {
            AccountId = claimTypes.AccountId,
            CompanyName = defaultFields.FirstOrDefault(x => x.FieldName == nameof(Company.CompanyName))?.FieldValue,
            LegalName = defaultFields.FirstOrDefault(x => x.FieldName == nameof(Company.LegalName))?.FieldValue,
            Logo = defaultFields.FirstOrDefault(x => x.FieldName == nameof(Company.Logo))?.FieldValue,
            LinkedInUrl = defaultFields.FirstOrDefault(x => x.FieldName == nameof(Company.LinkedInUrl))?.FieldValue,
            TwitterUrl = defaultFields.FirstOrDefault(x => x.FieldName == nameof(Company.TwitterUrl))?.FieldValue,
            FacebookUrl = defaultFields.FirstOrDefault(x => x.FieldName == nameof(Company.FacebookUrl))?.FieldValue,
            Website = defaultFields.FirstOrDefault(x => x.FieldName == nameof(Company.Website))?.FieldValue,
            PrimaryContact = defaultFields.FirstOrDefault(x => x.FieldName == nameof(Company.PrimaryContact))?.FieldValue.Split(',').ToList().FirstOrDefault(),
            CreatedDate = DateTime.Now,
            CreatedBy = claimTypes.UserId,
            EditedDate = DateTime.Now
        };

        // Add the company to the database
        await _unitOfWork.Company.Add(company);
        await _unitOfWork.Save();

        // Add the companycontact to the database
        var contactlist = defaultFields.FirstOrDefault(x => x.FieldName == nameof(Company.PrimaryContact))?.FieldValue.Split(',').ToList();
        if (contactlist != null)
        {
            List<CompanyContacts> companyContactsList = new List<CompanyContacts>();
            foreach (var contact in contactlist)
            {
                CompanyContacts companyContacts = new CompanyContacts()
                {
                    CompanyId = company.Id,
                    ContactId = int.Parse(contact),
                    IsPrimary = contact == contactlist[0] ? true : false,
                };
                companyContactsList.Add(companyContacts);
            }

            await _unitOfWork.CompanyContact.BulkAdd(companyContactsList);
            await _unitOfWork.Save();
        }

        // Handle custom fields
        var customFields = requestDTO.DataFields.Where(x => x.IsCustomField).ToList();
        foreach (var customField in customFields)
        {
            if (customField.FieldValue == null) continue;
            // Retrieve the custom field definition
            var existingCustomField = await _unitOfWork.CustomField.Get(x => x.FieldName == customField.FieldName && x.AccountId == claimTypes.AccountId);

            if (existingCustomField != null)
            {
                // Create a new StoreCustomFieldValue entity
                var storeCustomField = new StoreCustomFieldValue
                {
                    CustomFieldId = existingCustomField.Id,
                    TableName = existingCustomField.TableName,
                    TableId = company.Id,
                    StoreValue = customField.FieldValue
                };

                // Add the store custom field value to the database
                await _unitOfWork.CustomFieldStoreValue.Add(storeCustomField);
                // Save changes for custom fields
            }
        }
        await _unitOfWork.Save();
    }

    public async Task EditCompany(SettingCreateRequest requestDTO, CompanyDataRequestDTO companyData)
    {
        var company = await _unitOfWork.Company.Get(x => x.Id == companyData.CompanyId && x.AccountId == companyData.AccountId);
        if (company == null)
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
        if (company != null)
        {
            company.AccountId = companyData.AccountId.Value;
            company.CompanyName = defaultFieldlist.Where(x => x.FieldName == nameof(Company.CompanyName)).Select(x => x.FieldValue).FirstOrDefault() ?? company.CompanyName;
            company.LegalName = defaultFieldlist.Where(x => x.FieldName == nameof(Company.LegalName)).Select(x => x.FieldValue).FirstOrDefault() ?? company.LegalName;
            company.Logo = defaultFieldlist.Where(x => x.FieldName == nameof(Company.Logo)).Select(x => x.FieldValue).FirstOrDefault() ?? company.Logo;
            company.LinkedInUrl = defaultFieldlist.Where(x => x.FieldName == nameof(Company.LinkedInUrl)).Select(x => x.FieldValue).FirstOrDefault() ?? company.LinkedInUrl;
            company.TwitterUrl = defaultFieldlist.Where(x => x.FieldName == nameof(Company.TwitterUrl)).Select(x => x.FieldValue).FirstOrDefault() ?? company.TwitterUrl;
            company.FacebookUrl = defaultFieldlist.Where(x => x.FieldName == nameof(Company.FacebookUrl)).Select(x => x.FieldValue).FirstOrDefault() ?? company.FacebookUrl;
            company.Website = defaultFieldlist.Where(x => x.FieldName == nameof(Company.Website)).Select(x => x.FieldValue).FirstOrDefault() ?? company.Website;
            company.PrimaryContact = defaultFieldlist.Where(x => x.FieldName == nameof(Company.PrimaryContact)).Select(x => x.FieldValue).FirstOrDefault()?.Split(',').ToList()[0];
            company.EditedDate = DateTime.Now;
            company.EditedBy = companyData.AccountId.Value;
            await _unitOfWork.Company.Update(company);
        }
        var customFields = requestDTO.DataFields.Where(x => x.IsCustomField).ToList();
        foreach (var customField in customFields)
        {
            var getCustomField = await _unitOfWork.CustomField.Get(x => x.FieldName == customField.FieldName && x.AccountId == companyData.AccountId && x.TableName == customField.TableName);
            if (getCustomField != null)
            {
                var storeCustomFieldValue = await _unitOfWork.CustomFieldStoreValue.Get(x => x.CustomFieldId == getCustomField.Id && x.TableId == company.Id);
                if (storeCustomFieldValue != null)
                {
                    storeCustomFieldValue.StoreValue = customField.FieldValue;
                    await _unitOfWork.CustomFieldStoreValue.Update(storeCustomFieldValue);
                }

            }
        }
        await _unitOfWork.Save();

        var companyContactIdList = defaultFieldlist.Where(x => x.FieldName == nameof(Company.PrimaryContact)).Select(x => x.FieldValue).FirstOrDefault()?.Split(",").ToList();
        if (companyContactIdList != null)
        {
            await _unitOfWork.CompanyContact.SelectedRemove(x => x.CompanyId == company.Id && !companyContactIdList.Contains(x.ContactId.ToString()));
            await _unitOfWork.Save();
            var getCompantContactList = await _unitOfWork.CompanyContact.GetAll(x => x.CompanyId == company.Id);
            List<CompanyContacts> companyContactsList = new List<CompanyContacts>();
            List<CompanyContacts> updateCompanyContactsList = new List<CompanyContacts>();
            foreach (var contact in companyContactIdList)
            {
                var checkData = getCompantContactList.Where(x => x.ContactId.ToString() == contact).FirstOrDefault();
                if (checkData != null)
                {
                    if (checkData.ContactId.ToString() == companyContactIdList[0])
                    {
                        checkData.IsPrimary = true;
                    }
                    else
                    {
                        checkData.IsPrimary = false;
                    }
                    updateCompanyContactsList.Add(checkData);
                }
                else
                {
                    CompanyContacts companyContacts = new CompanyContacts()
                    {
                        CompanyId = company.Id,
                        ContactId = int.Parse(contact),
                        IsPrimary = contact == companyContactIdList[0] ? true : false,
                    };
                    companyContactsList.Add(companyContacts);
                }

            }
            if (companyContactIdList != null)
            {
                await _unitOfWork.CompanyContact.BulkAdd(companyContactsList);
                await _unitOfWork.Save();
            }
            if (updateCompanyContactsList != null)
            {
                await _unitOfWork.CompanyContact.BulkUpdate(updateCompanyContactsList);
                await _unitOfWork.Save();
            }

        }
        else
        {
            await _unitOfWork.CompanyContact.SelectedRemove(x => x.CompanyId == company.Id);
            await _unitOfWork.Save();
        }
    }

    public async Task DeleteCompany(CompanyDataRequestDTO requestDTO)
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

    public async Task CreateCompanyContact(CreateCompanyContact requestDTO, int accountId)
    {
        foreach (var image in requestDTO.Payload.Where(x => x.CustomFieldTypeName == "Image"))
        {

            var imageUpload = JsonSerializer.Deserialize<S3ImageUploadRequest>(image.FieldValue);
            imageUpload.Key = "logo";
            var result = await _services.S3Service.UploadS3File(imageUpload);
            image.FieldValue = result.FilePath;
        }

        // Retrieve all non-custom fields (default fields)
        var defaultFields = requestDTO.Payload.Where(x => !x.IsCustomField).ToList();

        // Create a new company object
        var contact = new Contact
        {
            AccountId = accountId,
            FirstName = defaultFields.FirstOrDefault(x => x.FieldName == nameof(Contact.FirstName))?.FieldValue,
            LastName = defaultFields.FirstOrDefault(x => x.FieldName == nameof(Contact.LastName))?.FieldValue,
            Phone = defaultFields.FirstOrDefault(x => x.FieldName == nameof(Contact.Phone))?.FieldValue,
            Email = defaultFields.FirstOrDefault(x => x.FieldName == nameof(Contact.Email))?.FieldValue,
            Mobile = defaultFields.FirstOrDefault(x => x.FieldName == nameof(Contact.Mobile))?.FieldValue,
            LinkedInUrl = defaultFields.FirstOrDefault(x => x.FieldName == nameof(Contact.LinkedInUrl))?.FieldValue,
            CreatedDate = DateTime.Now,
            CreatedBy = accountId
        };

        // Add the company to the database
        await _unitOfWork.Contact.Add(contact);
        await _unitOfWork.Save();

        // Handle custom fields
        var customFields = requestDTO.Payload.Where(x => x.IsCustomField).ToList();
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

        var companyContact = new CompanyContacts()
        {
            CompanyId = requestDTO.CompanyId,
            ContactId = contact.Id
        };

        await _unitOfWork.CompanyContact.Add(companyContact);
        await _unitOfWork.Save();

    }

    public async Task CreateCompanyNote(CreateCompanyNote requestDTO, int accountId)
    {
        var note = new Note()
        {
            Title = requestDTO.Title,
            Description = requestDTO.Description,
            CreatedBy = accountId,
            CreatedDate = DateTime.Now,
            EditedDate = DateTime.Now,
        };

        await _unitOfWork.Note.Add(note);
        await _unitOfWork.Save();

        var companyNote = new CompanyNotes()
        {
            CompanyId = requestDTO.CompanyId,
            NoteId = note.Id,
        };

        await _unitOfWork.CompanyNote.Add(companyNote);
        await _unitOfWork.Save();
    }

    public async Task CreateCompanyDocument(CreateCompanyDocument requestDTO, int accountId)
    {
        var document = new Document()
        {
            Name = requestDTO.Name,
            Description = requestDTO.Description,
            CreatedBy = accountId,
            CreatedDate = DateTime.Now,
            EditedDate = DateTime.Now,

        };
        if (requestDTO.FileRequest != null)
        {
            requestDTO.FileRequest.Key = "CompanyDocument";
            var result = await _services.S3Service.UploadS3File(requestDTO.FileRequest);
            document.FilePath = result.FilePath;
            document.Type = requestDTO.FileRequest.FileExtension;
        }
        else
        {
            throw new BusinessLogicException("404", "Please Upload valid fiel.");
        }

        await _unitOfWork.Document.Add(document);
        await _unitOfWork.Save();
        var companyDocument = new CompanyDocuments()
        {
            CompanyId = requestDTO.CompanyId,
            DocumentId = document.Id,
        };
        await _unitOfWork.CompanyDocument.Add(companyDocument);
        await _unitOfWork.Save();
    }
}

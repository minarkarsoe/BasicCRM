using AutoMapper;
using Recsite_Ats.Application.Common.Interface.Businesslogic;
using Recsite_Ats.Application.Common.Interface.Repository;
using Recsite_Ats.Application.Common.Interface.Services;
using Recsite_Ats.Domain.API.APIRequest;
using Recsite_Ats.Domain.DataTransferObject;
using Recsite_Ats.Domain.Entites;
using Recsite_Ats.Domain.ViewModels;
using Recsite_Ats.Infrastructure.CustomException;

namespace Recsite_Ats.Infrastructure.BusinessLogic;
public class AdminSettingBusinessLogic : IAdminSettingBusinessLogic
{
    private readonly IServices _services;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public AdminSettingBusinessLogic(IServices services, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _services = services;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SectionLayoutDTO> GetSetting(string tableName, int? accountId, string columnNames)
    {
        var fieldTypes = await _unitOfWork.FieldType.GetAll();
        var result = await _services.SettingService.GetFieldDetailsAsync(tableName, accountId, columnNames);
        var customFields = await _unitOfWork.CustomField.GetAll(cf => cf.AccountId == accountId && cf.TableName == result.Sections[0].TableName, includeProperties: "FieldType");
        result.FieldTypes = fieldTypes.Select(x => new FieldTypesViewModel
        {
            FieldTypeId = x.Id,
            FieldTypeName = x.FieldTypeName,
        }).ToList();
        result.CustomFieldLists = customFields.Select(x => new CustomFieldsViewModel
        {
            CustomFieldId = x.Id,
            FieldName = x.FieldName,
            FieldTypeId = x.FieldTypeId,
            FieldTypeName = x.FieldType.FieldTypeName,
            ViewValue = x.ViewValues
        }).ToList();

        return result;
    }
    public async Task<bool> UpdateSetting(UpdateOrderRequest request)
    {
        var updateFieldLayouts = new List<FieldLayout>();
        var insertFieldLayouts = new List<FieldLayout>();
        foreach (var item in request.CustomFields)
        {
            FieldLayout fieldLayout;

            if (item.Id.HasValue && item.Id.Value != 0)
            {
                fieldLayout = await _unitOfWork.FieldLayout.Get(f => f.Id == item.Id);

                if (fieldLayout == null)
                {
                    throw new BusinessLogicException("404", $"FieldLayout with ID {item.Id} not found.");
                }

                updateFieldLayouts.Add(_mapper.Map<FieldLayout>(item));
            }
            else
            {
                // Create a new field layout for insertion
                fieldLayout = _mapper.Map<FieldLayout>(item);
                insertFieldLayouts.Add(fieldLayout);
            }

        }

        foreach (var section in request.Sections)
        {
            SectionLayout sectionLayout = _mapper.Map<SectionLayout>(section);
            await _unitOfWork.Section.Update(sectionLayout);
            await _unitOfWork.Save();
        }

        if (insertFieldLayouts.Any())
        {
            await _unitOfWork.FieldLayout.BulkAdd(insertFieldLayouts);
            await _unitOfWork.Save();
        }
        // Bulk update existing field layouts
        if (updateFieldLayouts.Any())
        {
            await _unitOfWork.FieldLayout.BulkUpdate(updateFieldLayouts);
            await _unitOfWork.Save();
        }

        return true;
    }

    public async Task<SectionLayout> CreateSection(CreateSectionRequest request, int accountId)
    {
        var section = await _unitOfWork.Section.Get(s => s.SectionName == request.SectionName && s.TableName == request.TableName && (s.AccountId == accountId || s.AccountId == null));
        if (section != null)
        {
            throw new BusinessLogicException("403", "Section is already existed.");
        }

        // Fetch the maximum SortOrder for the account
        int maxSortOrder = await _unitOfWork.Section.Max(s => s.Sort, s => (s.AccountId == accountId || s.AccountId == null) && s.IsCustomSection && s.TableName == request.TableName);

        var newSection = new SectionLayout()
        {
            SectionName = request.SectionName,
            TableName = request.TableName,
            AccountId = accountId,
            IsCustomSection = true,
            Visible = true,
            Sort = maxSortOrder + 1 // Increment the max sort order for the new section
        };

        await _unitOfWork.Section.Add(newSection);
        await _unitOfWork.Save();
        return newSection;
    }

    public async Task<bool> UpdateSection(UpdateSectionRequest request, int accountId)
    {
        var section = await _unitOfWork.Section.Get(s => s.SectionLayoutId == request.SectionId && s.AccountId == accountId);
        if (section == null)
        {
            throw new BusinessLogicException("404", "Section is not found!");
        }
        section.SectionName = request.SectionName;
        await _unitOfWork.Section.Update(section);
        await _unitOfWork.Save();
        return true;
    }

    public async Task<bool> DeleteSection(DeleteSectionRequest request, int accountId)
    {
        var section = await _unitOfWork.Section.Get(s => s.SectionLayoutId == request.SectionId && s.AccountId == accountId);
        if (section == null)
        {
            throw new BusinessLogicException("404", "Section is not found!");
        }
        var fieldLayoutsBySections = await _unitOfWork.FieldLayout.GetAll(f => f.SectionLayoutId == request.SectionId && f.AccountId == accountId);
        if (fieldLayoutsBySections == null || fieldLayoutsBySections.Count() == 0)
        {
            _unitOfWork.Section.Remove(section);
            await _unitOfWork.Save();
            return true;
        }
        var defaultSection = await _unitOfWork.Section.Get(s => !s.IsCustomSection && s.TableName == section.TableName);
        int getMaxSortOrder = await _unitOfWork.FieldLayout.Max(f => (int)f.Sort, f => f.SectionLayoutId == defaultSection.SectionLayoutId && f.AccountId == accountId);
        foreach (var fieldLayout in fieldLayoutsBySections)
        {
            fieldLayout.SectionLayoutId = defaultSection.SectionLayoutId;
            fieldLayout.Sort = getMaxSortOrder + 1;
            await _unitOfWork.FieldLayout.Update(fieldLayout);
            getMaxSortOrder++;
        }
        _unitOfWork.Section.Remove(section);

        var sectionLayouts = await _unitOfWork.Section.GetAll(f => f.TableName == section.TableName && f.AccountId == accountId && f.IsCustomSection && f.SectionName != section.SectionName);
        int index = 1;
        foreach (var sectionLayout in sectionLayouts.OrderBy(s => s.Sort))
        {
            sectionLayout.Sort = index;
            await _unitOfWork.Section.Update(sectionLayout);
            index++;
        }
        await _unitOfWork.Save();
        return true;
    }

    public async Task<CustomField> CreateCustomField(CreateCustomFieldRequest request, int accountId)
    {
        var fieldTypes = await _unitOfWork.FieldType.Get(x => x.Id == request.FieldTypeId);
        if (fieldTypes.FieldTypeName.ToLower() == "MultiSelect".ToLower() || fieldTypes.FieldTypeName.ToLower() == "DropDown".ToLower())
        {
            if (string.IsNullOrEmpty(request.ViewValue))
            {
                throw new BusinessLogicException("400", "Comma seperated value can't be null!");
            }
        }
        var defaultSection = await _unitOfWork.Section.Get(s => !s.IsCustomSection && s.TableName == request.TableName);
        int getMaxSortOrder = await _unitOfWork.FieldLayout.Max(f => (int)f.Sort, f => f.SectionLayoutId == defaultSection.SectionLayoutId && f.AccountId == accountId);
        var fieldLayOut = new FieldLayout()
        {
            AccountId = accountId,
            TableName = request.TableName,
            FieldName = request.FieldName,
            SectionLayoutId = defaultSection.SectionLayoutId,
            Sort = getMaxSortOrder == 0 ? 9999 : getMaxSortOrder + 1,
            Required = false,
            Visible = true,
            IsCustomField = true,
        };
        await _unitOfWork.FieldLayout.Add(fieldLayOut);
        var customField = new CustomField()
        {
            AccountId = accountId,
            TableName = request.TableName,
            FieldName = request.FieldName,
            FieldAlias = "_" + request.FieldName.ToLower(),
            FieldTypeId = request.FieldTypeId,
            ViewValues = request.ViewValue,
        };
        var response = await _unitOfWork.CustomField.Add(customField);
        await _unitOfWork.Save();
        return response;
    }

    public async Task<CustomField> UpdateCustomField(UpdateCustomFieldRequest request, int accountId)
    {
        var customField = await _unitOfWork.CustomField.Get(c => c.Id == request.CustomFieldId && c.AccountId == accountId);
        if (customField == null)
        {
            throw new BusinessLogicException("404", "Custom Field's not found!");
        }
        var fieldLayout = await _unitOfWork.FieldLayout.Get(f => f.FieldName == customField.FieldName && f.TableName == customField.TableName && f.AccountId == accountId);
        customField.FieldName = request.FieldName;
        customField.ViewValues = request.ViewValue;
        customField.FieldAlias = "_" + request.FieldName.ToLower();
        if (fieldLayout != null)
        {
            fieldLayout.FieldName = customField.FieldName;
            await _unitOfWork.FieldLayout.Update(fieldLayout);
        }
        var response = await _unitOfWork.CustomField.UpdateAndGetData(customField);
        return response;
    }

    public async Task<bool> DeleteCustomField(DeleteCustomFieldRequest request, int accountId)
    {
        var customField = await _unitOfWork.CustomField.Get(c => c.Id == request.CustomFieldId && c.FieldName == request.FieldName && c.AccountId == accountId);
        if (customField == null)
        {
            throw new BusinessLogicException("404", "Custom Field's not found!");
        }
        var fieldLayoutByCustomField = await _unitOfWork.FieldLayout.Get(f => f.TableName == customField.TableName && f.AccountId == accountId && f.FieldName == customField.FieldName);
        if (fieldLayoutByCustomField != null)
        {
            _unitOfWork.FieldLayout.Remove(fieldLayoutByCustomField);
        }
        _unitOfWork.CustomField.Remove(customField);
        await _unitOfWork.Save();
        return true;
    }
}

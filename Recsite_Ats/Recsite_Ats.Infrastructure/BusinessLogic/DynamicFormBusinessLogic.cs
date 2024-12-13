using Recsite_Ats.Application.Common.Interface.Businesslogic;
using Recsite_Ats.Application.Common.Interface.Repository;
using Recsite_Ats.Application.Common.Interface.Services;
using Recsite_Ats.Domain.DataTransferObject;

namespace Recsite_Ats.Infrastructure.BusinessLogic;
public class DynamicFormBusinessLogic : IDynamicFormBusinessLogic
{
    private readonly IServices _services;
    private readonly IUnitOfWork _unitOfWork;
    public DynamicFormBusinessLogic(IServices services, IUnitOfWork unitOfWork)
    {
        _services = services;
        _unitOfWork = unitOfWork;
    }

    public async Task<DynamicFormLayoutDTO> GetDynamicFormData(string tableName, int? accountId, string columnNames)
    {
        var result = await _services.SettingService.GetFieldDetailsAsync(tableName, accountId, columnNames);
        foreach (var field in result.CustomFields.Where(x => x.IsCustomField))
        {
            if (field.CustomFieldTypeId == 5 && field.CustomFieldTypeName == "MultiSelect")
            {
                var customField = await _unitOfWork.CustomField.Get(customField => customField.FieldName == field.FieldName);
                if (customField != null)
                {
                    field.SelectedOptions = customField.ViewValues?.Split(',').ToList();
                }
            }
            else if (field.CustomFieldTypeId == 4 && field.CustomFieldTypeName == "DropDown")
            {
                var customField = await _unitOfWork.CustomField.Get(customField => customField.FieldName == field.FieldName);
                if (customField != null)
                {
                    field.SelectedOptions = customField.ViewValues?.Split(',').ToList();
                }
            }
        }
        DynamicFormLayoutDTO dynamicFormLayoutDTO = new DynamicFormLayoutDTO()
        {
            Sections = result.Sections,
            CustomFields = result.CustomFields.Where(field => field.IsVisible).ToList(),
        };

        return dynamicFormLayoutDTO;
    }
}

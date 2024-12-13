using Recsite_Ats.Domain.DataTransferObject;
using Recsite_Ats.Domain.ViewModels;

namespace Recsite_Ats.Domain.API.APIRequest;
public class AdminSettingRequest
{
    public string TableName { get; set; }
}
public class CreateSectionRequest
{
    public string TableName { get; set; }
    public string SectionName { get; set; }
}
public class UpdateSectionRequest
{
    public int SectionId { get; set; }
    public string SectionName { get; set; }
}

public class DeleteSectionRequest
{
    public int SectionId { get; set; }
    public string SectionName { get; set; }
}

public class CreateCustomFieldRequest
{
    public string FieldName { get; set; }
    public int FieldTypeId { get; set; }
    public string? ViewValue { get; set; }
    public string TableName { get; set; }
}

public class UpdateCustomFieldRequest
{
    public int CustomFieldId { get; set; }
    public string? FieldName { get; set; }
    public string? ViewValue { get; set; }
}

public class DeleteCustomFieldRequest
{
    public int CustomFieldId { get; set; }
    public string FieldName { get; set; }
}
public class UpdateOrderRequest
{
    public List<CustomFieldViewModel> CustomFields { get; set; }
    public List<SectionDTO> Sections { get; set; }
}

public class SettingCreateRequest
{
    public List<CustomFieldsDTO> DataFields { get; set; }
}

public class Country
{
    public string ISO_Code { get; set; }
    public string Name { get; set; }
}

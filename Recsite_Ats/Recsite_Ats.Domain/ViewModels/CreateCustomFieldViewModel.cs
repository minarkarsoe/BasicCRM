namespace Recsite_Ats.Domain.ViewModels;
public class CreateCustomFieldViewModel
{
    public string FieldName { get; set; }
    public int FieldTypeId { get; set; }
    public string? ViewValue { get; set; }
    public string? TableName { get; set; }
    public string? ReturnUrl { get; set; }
    public List<FieldTypesViewModel>? FieldTypes { get; set; }
}

public class EditCustomerFieldViewModel
{
    public int CustomFieldId { get; set; }
    public string? FieldName { get; set; }
    public string? ViewValue { get; set; }
}

public class FieldTypesViewModel
{
    public int FieldTypeId { get; set; }
    public string FieldTypeName { get; set; }
}

public class CustomFieldsViewModel
{
    public int CustomFieldId { get; set; }
    public string FieldName { get; set; }
    public int FieldTypeId { get; set; }
    public string FieldTypeName { get; set; }
    public string ViewValue { get; set; }

}

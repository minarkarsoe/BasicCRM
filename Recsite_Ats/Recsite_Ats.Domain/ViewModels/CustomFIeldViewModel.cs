namespace Recsite_Ats.Domain.ViewModels;

public class CustomFieldViewModel
{
    public string TableName { get; set; }
    public int AccountId { get; set; }
    public int? Id { get; set; }
    public string FieldName { get; set; }
    public string CustomFieldTypeName { get; set; }
    public int CustomFieldTypeId { get; set; }
    public int SectionLayoutId { get; set; }
    public bool IsNullable { get; set; }
    public bool IsLocked { get; set; }
    public bool IsRequired { get; set; }
    public bool IsVisible { get; set; }
    public int SortOrder { get; set; }
    public bool IsCustomField { get; set; }
}

using Microsoft.AspNetCore.Mvc.Rendering;
using Recsite_Ats.Domain.API.APIRequest;
using Recsite_Ats.Domain.ViewModels;

namespace Recsite_Ats.Domain.DataTransferObject;
public class CustomFieldsDTO
{
    public int? Id { get; set; }
    public long? Index { get; set; }
    public string? TableName { get; set; }
    public int? AccountId { get; set; }
    public string FieldName { get; set; }
    public string? FieldValue { get; set; }
    public string? Base64String { get; set; }
    public S3ImageUploadRequest? ImageData { get; set; }
    public List<SelectListItem>? AvailableOptions { get; set; }
    public List<string>? SelectedOptions { get; set; }
    public int CustomFieldTypeId { get; set; }
    public string CustomFieldTypeName { get; set; }
    public int SectionLayoutId { get; set; }
    public bool IsNullable { get; set; }
    public bool IsLocked { get; set; }
    public bool IsRequired { get; set; }
    public bool IsVisible { get; set; }
    public long SortOrder { get; set; }
    public bool IsCustomField { get; set; }
}

public class SectionDTO
{
    public int SectionLayoutId { get; set; }
    public int? AccountId { get; set; }
    public string TableName { get; set; }
    public string SectionName { get; set; }
    public int Sort { get; set; }
    public bool Visible { get; set; }
    public bool IsCustomSection { get; set; }
}

public class SectionLayoutDTO
{
    public List<SectionDTO> Sections { get; set; }
    public List<CustomFieldsDTO> CustomFields { get; set; }
    public List<FieldTypesViewModel>? FieldTypes { get; set; }
    public List<CustomFieldsViewModel>? CustomFieldLists { get; set; }
}

public class DynamicFormLayoutDTO
{
    public List<SectionDTO> Sections { get; set; }
    public List<CustomFieldsDTO> CustomFields { get; set; }
}

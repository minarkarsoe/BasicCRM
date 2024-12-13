using Recsite_Ats.Domain.DataTransferObject;

namespace Recsite_Ats.Domain.API.APIResponse;

public class SettingDataAPIResponse
{
    public List<SectionDTO> Sections { get; set; }
    public List<CustomFieldsDTO> CustomFields { get; set; }
}

public class SearchData
{
    public int? Id { get; set; }
    public string? Name { get; set; }
}

public class SearchDataResponse
{
    public List<SearchData> SearchData { get; set; }
}

public class CountryDataResponse
{
    public string ISO_Code { get; set; }
    public string Name { get; set; }
}

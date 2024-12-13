using Recsite_Ats.Domain.DataTransferObject;

namespace Recsite_Ats.Domain.API.APIResponse;
public class CompanyDetailDataResponse
{
    public List<SectionDTO> Sections { get; set; }
    public List<CustomFieldsDTO> CustomFields { get; set; }

    public CompanyDetailDTO CompanyDetails { get; set; }
    public List<CompanyFollwersDTO> CompanyFollowers { get; set; }
}

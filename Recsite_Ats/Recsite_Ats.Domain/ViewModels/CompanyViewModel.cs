using Microsoft.AspNetCore.Http;

namespace Recsite_Ats.Domain.ViewModels;

public class CompanyViewModel
{
    public int AccountId { get; set; }
    public string CompanyName { get; set; }
    public string LegalName { get; set; }
    public int ContentId { get; set; }
    public int ParentCompanyId { get; set; }
    public IFormFile Logo { get; set; }
    public string LinkedInUrl { get; set; }
    public string TwitterUrl { get; set; }
    public string FacebookUrl { get; set; }
    public string? Website { get; set; }
}

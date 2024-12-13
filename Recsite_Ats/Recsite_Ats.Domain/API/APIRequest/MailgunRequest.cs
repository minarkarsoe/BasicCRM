using System.ComponentModel.DataAnnotations;

namespace Recsite_Ats.Domain.API.APIRequest;

public class GetDomainKeyRequest
{
    public string? domain_name { get; set; }
    public string? kind { get; set; }
}

public class AddDomainKeyRequest
{
    [Required]
    public string role { get; set; }
    public string? email { get; set; }
    public string? domain_name { get; set; }
    public string? kind { get; set; }
    public int? expiration { get; set; }
    public string? user_id { get; set; }
    public string? description { get; set; }
}

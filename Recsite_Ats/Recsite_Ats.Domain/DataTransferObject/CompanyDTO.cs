using Recsite_Ats.Domain.API.APIRequest;
using Recsite_Ats.Domain.Entites;

namespace Recsite_Ats.Domain.DataTransferObject;
public class CompanyDataResponseDTO
{
    public IEnumerable<Company>? Companies { get; set; }
    public SectionLayoutDTO? SectionLayout { get; set; }
}

public class CompanyDataRequestDTO
{
    public string TableName { get; set; }
    public int? AccountId { get; set; }
    public string Columns { get; set; }
    public int? CompanyId { get; set; }
}


public class CompanyDetailDTO
{
    public int CompanyId { get; set; }
    public string? Logo { get; set; }
    public string CompanyName { get; set; }
    public DateTime? LastUpdated { get; set; }
    public string MobileNumber { get; set; }
    public string Location { get; set; }
    public List<ContactDetailDTO> Contacts { get; set; }
}

public class CompanyFollwersDTO
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public bool IsOwner { get; set; } = false;
}

public class BillInformationDTO
{
    public string CompanyName { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string City { get; set; }
    public Country Country { get; set; }
    public string ZipCode { get; set; }
    public string? VATNumber { get; set; }

}

public class PaymentMethodDTO
{
    public string Name { get; set; }
    public string CardNumber { get; set; }
    public string SecurityCode { get; set; }
    public string ExpiryMonth { get; set; }
    public string ExpiryYear { get; set; }
}
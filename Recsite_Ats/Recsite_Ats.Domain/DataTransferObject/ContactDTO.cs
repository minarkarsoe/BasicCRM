using Recsite_Ats.Domain.Entites;

namespace Recsite_Ats.Domain.DataTransferObject;
public class ContactDataResponseDTO
{
    public IEnumerable<Contact>? Contacts { get; set; }
    public SectionLayoutDTO? SectionLayout { get; set; }
}

public class ContactDataRequestDTO
{
    public string TableName { get; set; }
    public int? AccountId { get; set; }
    public string Columns { get; set; }
    public int? ContactId { get; set; }
}

public class ContactDetailDTO
{
    public int ContactId { get; set; }
    public string ContactName { get; set; }
    public string MobileNumber { get; set; }
}


using Recsite_Ats.Domain.DataTransferObject;

namespace Recsite_Ats.Domain.API.APIRequest;
public class CreateCompanyContact
{
    public List<CustomFieldsDTO> Payload { get; set; }
    public int CompanyId { get; set; }

}

public class CreateCompanyNote
{
    public int CompanyId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}

public class CreateCompanyDocument
{
    public int CompanyId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public S3ImageUploadRequest FileRequest { get; set; }
}

namespace Recsite_Ats.Domain.API.APIRequest;

public class S3ImageUploadRequest
{
    public string Base64ImageData { get; set; }
    public string FileExtension { get; set; }
    public string Key { get; set; }
}

public class S3ImageDeleteRequest
{
    public string FilePath { get; set; }
}

public class GetS3ImageRequest
{
    public string FilePath { get; set; }
}

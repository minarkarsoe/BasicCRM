using Microsoft.AspNetCore.Http;
using Recsite_Ats.Domain.API.APIRequest;
using Recsite_Ats.Domain.API.APIResponse;

namespace Recsite_Ats.Application.Common.Interface.Services;
public interface IS3Service
{
    Task<string> GetBase64ImageStringAsync(string fileName);

    Task<string> UploadFileAsync(IFormFile file);

    Task<S3UploadResponse> UploadS3File(S3ImageUploadRequest request);

    Task<bool> DeleteFileAsync(string fileKey);
}

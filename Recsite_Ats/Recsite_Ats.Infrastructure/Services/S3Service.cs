using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Recsite_Ats.Application.Common.Interface.Services;
using Recsite_Ats.Domain.API.APIRequest;
using Recsite_Ats.Domain.API.APIResponse;

namespace Recsite_Ats.Infrastructure.Services;
public class S3Service : IS3Service
{
    private readonly string _bucketName;
    private readonly string _accessKey;
    private readonly string _secretKey;
    public S3Service(IConfiguration configuration)
    {
        _bucketName = configuration["AWS:BucketName"];
        _accessKey = configuration["AWS:AccessKeyId"];
        _secretKey = configuration["AWS:SecretAccessKey"];
    }
    public async Task<string> GetBase64ImageStringAsync(string fileName)
    {
        var getRequest = new GetObjectRequest
        {
            BucketName = _bucketName,
            Key = fileName
        };

        var config = new AmazonS3Config
        {
            Timeout = TimeSpan.FromSeconds(10),
            ReadWriteTimeout = TimeSpan.FromSeconds(10),
            RetryMode = RequestRetryMode.Standard,
            MaxErrorRetry = 2,
            RegionEndpoint = RegionEndpoint.APSoutheast2
        };

        using (var s3Client = new AmazonS3Client(new BasicAWSCredentials(_accessKey, _secretKey), config))
        {
            using (var response = await s3Client.GetObjectAsync(getRequest))
            {
                using (var memoryStream = new MemoryStream())
                {
                    await response.ResponseStream.CopyToAsync(memoryStream);
                    byte[] fileBytes = memoryStream.ToArray();
                    string base64String = Convert.ToBase64String(fileBytes);

                    // Get the file extension to include in the base64 data
                    string extension = Path.GetExtension(fileName).TrimStart('.');
                    string mimeType = $"image/{extension}";

                    return $"data:{mimeType};base64,{base64String}";
                }
            }
        }
    }


    public async Task<string> UploadFileAsync(IFormFile file)
    {
        var fileName = file.FileName;
        var fileExtension = Path.GetExtension(fileName);
        var contentType = file.ContentType;

        // Create a MemoryStream to hold the file's content
        using (var memoryStream = new MemoryStream())
        {
            // Copy the file stream into the memory stream
            await file.CopyToAsync(memoryStream);

            // Reset the position to the beginning of the stream
            memoryStream.Position = 0;

            var config = new AmazonS3Config
            {
                Timeout = TimeSpan.FromSeconds(10),
                ReadWriteTimeout = TimeSpan.FromSeconds(10),
                RetryMode = RequestRetryMode.Standard,
                MaxErrorRetry = 2,
                RegionEndpoint = RegionEndpoint.APSoutheast2
            };

            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = "logo/" + Guid.NewGuid() + fileExtension,
                InputStream = memoryStream,
                ContentType = contentType
            };

            // Upload file to S3 using AmazonS3Client
            var s3Client = new AmazonS3Client(new BasicAWSCredentials(_accessKey, _secretKey), config);

            await s3Client.PutObjectAsync(putRequest);
            return putRequest.Key;
        }
    }

    public async Task<S3UploadResponse> UploadS3File(S3ImageUploadRequest request)
    {
        var response = new S3UploadResponse();
        var photoByteArray = Convert.FromBase64String(request.Base64ImageData); /*Helper.GetOneMbImage(request.Base64ImageData);*/
        request.Key = $"{request.Key}/{Guid.NewGuid()}.{request.FileExtension}";
        var config = new AmazonS3Config
        {
            Timeout = TimeSpan.FromSeconds(10),
            ReadWriteTimeout = TimeSpan.FromSeconds(10),
            RetryMode = RequestRetryMode.Standard,
            MaxErrorRetry = 2,
            RegionEndpoint = RegionEndpoint.APSoutheast2
        };
        IAmazonS3 s3Client = new AmazonS3Client(new BasicAWSCredentials(_accessKey, _secretKey), config);
        using (var fileTransferUtility = new TransferUtility(s3Client))
        {
            var S3Upload = new TransferUtilityUploadRequest
            {
                BucketName = _bucketName,
                Key = request.Key,
                InputStream = new MemoryStream(photoByteArray),
                AutoCloseStream = true
            };
            fileTransferUtility.Upload(S3Upload);
        }
        response.FilePath = request.Key;
        return response;
    }

    public async Task<bool> DeleteFileAsync(string fileKey)
    {
        var config = new AmazonS3Config
        {
            Timeout = TimeSpan.FromSeconds(10),
            ReadWriteTimeout = TimeSpan.FromSeconds(10),
            RetryMode = RequestRetryMode.Standard,
            MaxErrorRetry = 2,
            RegionEndpoint = RegionEndpoint.APSoutheast2
        };

        using (var s3Client = new AmazonS3Client(new BasicAWSCredentials(_accessKey, _secretKey), config))
        {
            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = fileKey // This is the key of the file to delete
            };

            var response = await s3Client.DeleteObjectAsync(deleteRequest);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }

            return false;

        }
    }

}

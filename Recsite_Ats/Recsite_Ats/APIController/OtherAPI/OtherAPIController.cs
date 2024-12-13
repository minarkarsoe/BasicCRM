using Amazon.Runtime;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recsite_Ats.Application.Common.Helper;
using Recsite_Ats.Application.Common.Interface.Businesslogic;
using Recsite_Ats.Application.Common.Interface.Repository;
using Recsite_Ats.Application.Common.Interface.Services;
using Recsite_Ats.Domain.API;
using Recsite_Ats.Domain.API.APIRequest;
using Recsite_Ats.Domain.DataTransferObject;
using System.Text.Json;

namespace Recsite_Ats.Web.APIController.OtherAPI;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/aws/s3-image/")]
public class OtherAPIController : BaseAPIController
{
    public OtherAPIController(
       IUnitOfWork unitOfWork,
       IServices services,
       IBusinessLogic businessLogic,
       ClaimHelper claimHelper
       ) : base(unitOfWork, services, businessLogic, claimHelper)
    {

    }
    [Authorize]
    [HttpPost("upload")]
    public async Task<BaseAPIResponse> UploadS3Imgae(S3ImageUploadRequest request)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var claims = User.Claims.Where(x => x.Type == "AccountId").FirstOrDefault()?.Value;
            if (string.IsNullOrEmpty(claims))
            {
                response.RespCode = "404";
                response.RespDescription = "Invalid User! User is not Found";
                return response;
            }

            int accountId = int.Parse(claims);

            var result = await _services.S3Service.UploadS3File(request);

            response.Data = result;

            return response;
        }
        catch (FormatException ex)
        {
            // Handle invalid Base64 format exceptions

            response.RespCode = "400";
            response.RespDescription = $"Invalid image data format: {ex.Message}";
            return response;
        }
        catch (ArgumentException ex)
        {
            // Handle invalid image format or stream errors
            response.RespCode = "400";
            response.RespDescription = $"Invalid image format: {ex.Message}";
            return response;
        }
        catch (AmazonClientException ex)
        {
            response.RespCode = "500";
            response.RespDescription = ex.Message;
            return response;
        }
        catch (Exception ex)
        {
            response.RespCode = "500";
            response.RespDescription = ex.Message;
            return response;
        }
        finally
        {
            LoggingDTO log = new LoggingDTO()
            {
                ControllerName = nameof(OtherAPIController),
                ActionName = nameof(UploadS3Imgae),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [Authorize]
    [HttpDelete("delete")]
    public async Task<BaseAPIResponse> DeleteS3Image(S3ImageDeleteRequest request)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var claims = User.Claims.Where(x => x.Type == "AccountId").FirstOrDefault()?.Value;
            if (string.IsNullOrEmpty(claims))
            {
                response.RespCode = "404";
                response.RespDescription = "Invalid User! User is not Found";
                return response;
            }

            int accountId = int.Parse(claims);

            var result = await _services.S3Service.DeleteFileAsync(request.FilePath);
            if (!result)
            {
                response.RespCode = "404";
                response.RespDescription = "Faile to delete the resource. Resource is not found!";
                return response;
            }

            return response;
        }
        catch (AmazonClientException ex)
        {
            response.RespCode = "500";
            response.RespDescription = ex.Message;
            return response;
        }
        catch (Exception ex)
        {
            response.RespCode = "500";
            response.RespDescription = ex.Message;
            return response;
        }
        finally
        {
            LoggingDTO log = new LoggingDTO()
            {
                ControllerName = nameof(OtherAPIController),
                ActionName = nameof(DeleteS3Image),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [Authorize]
    [HttpPost("get-image")]
    public async Task<BaseAPIResponse> GetS3Image(GetS3ImageRequest request)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var claims = User.Claims.Where(x => x.Type == "AccountId").FirstOrDefault()?.Value;
            if (string.IsNullOrEmpty(claims))
            {
                response.RespCode = "404";
                response.RespDescription = "Invalid User! User is not Found";
                return response;
            }

            int accountId = int.Parse(claims);
            var result = await _services.S3Service.GetBase64ImageStringAsync(request.FilePath);
            response.Data = result;
            return response;
        }
        catch (AmazonClientException ex)
        {
            response.RespCode = "500";
            response.RespDescription = ex.Message;
            return response;
        }
        catch (Exception ex)
        {
            response.RespCode = "500";
            response.RespDescription = ex.Message;
            return response;
        }
        finally
        {
            LoggingDTO log = new LoggingDTO()
            {
                ControllerName = nameof(OtherAPIController),
                ActionName = nameof(GetS3Image),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }

    }
}

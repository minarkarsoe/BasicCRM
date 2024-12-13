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
using Recsite_Ats.Infrastructure.CustomException;
using System.Text.Json;

namespace Recsite_Ats.Web.APIController.MailGun;
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
public class MailGunController : BaseAPIController
{
    private readonly IEmailSender _emailSender;
    public MailGunController(
        IUnitOfWork unitOfWork,
        IServices services,
        IBusinessLogic businessLogic,
        ClaimHelper claimHelper,
        IEmailSender emailSender
        ) : base(unitOfWork, services, businessLogic, claimHelper)
    {
        _emailSender = emailSender;
    }

    [HttpPost("add-domain")]
    [Authorize]
    public async Task<BaseAPIResponse> AddDomain(AddDomain request)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var claimTypes = _claimHelper.CheckAccountValidorNot();

            var result = await _emailSender.AddDomain(request.domain);

            response.Data = result;
            return response;


        }
        catch (BusinessLogicException err)
        {
            response.RespCode = err.ResponseCode;
            response.RespDescription = err.Message;
            return response;
        }
        catch (Exception ex)
        {
            response.RespDescription = ex.Message;
            response.RespCode = "500";
            return response;
        }
        finally
        {
            LoggingDTO log = new LoggingDTO()
            {
                ControllerName = nameof(JobController),
                ActionName = nameof(AddDomain),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [HttpPost("get-domain")]
    [Authorize]
    public async Task<BaseAPIResponse> GetDomain(AddDomain request)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var claimTypes = _claimHelper.CheckAccountValidorNot();

            var result = await _emailSender.GetDoaminDetails(request.domain);

            response.Data = result;
            return response;


        }
        catch (BusinessLogicException err)
        {
            response.RespCode = err.ResponseCode;
            response.RespDescription = err.Message;
            return response;
        }
        catch (Exception ex)
        {
            response.RespDescription = ex.Message;
            response.RespCode = "500";
            return response;
        }
        finally
        {
            LoggingDTO log = new LoggingDTO()
            {
                ControllerName = nameof(JobController),
                ActionName = nameof(AddDomain),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [HttpPost("get-domain-key")]
    [Authorize]
    public async Task<BaseAPIResponse> GetDomainKey(GetDomainKeyRequest request)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var claimTypes = _claimHelper.CheckAccountValidorNot();

            var result = await _emailSender.GetDomainKey(request);

            response.Data = result;
            return response;


        }
        catch (BusinessLogicException err)
        {
            response.RespCode = err.ResponseCode;
            response.RespDescription = err.Message;
            return response;
        }
        catch (Exception ex)
        {
            response.RespDescription = ex.Message;
            response.RespCode = "500";
            return response;
        }
        finally
        {
            LoggingDTO log = new LoggingDTO()
            {
                ControllerName = nameof(JobController),
                ActionName = nameof(AddDomain),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [HttpPost("add-domain-key")]
    [Authorize]
    public async Task<BaseAPIResponse> AddDomainKey(AddDomainKeyRequest request)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var claimTypes = _claimHelper.CheckAccountValidorNot();

            var result = await _emailSender.AddDomainKey(request, claimTypes);

            response.Data = result;
            return response;

        }
        catch (BusinessLogicException err)
        {
            response.RespCode = err.ResponseCode;
            response.RespDescription = err.Message;
            return response;
        }
        catch (Exception ex)
        {
            response.RespDescription = ex.Message;
            response.RespCode = "500";
            return response;
        }
        finally
        {
            LoggingDTO log = new LoggingDTO()
            {
                ControllerName = nameof(JobController),
                ActionName = nameof(AddDomain),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }
}

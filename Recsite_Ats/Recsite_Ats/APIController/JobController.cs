using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recsite_Ats.Application.Common.Helper;
using Recsite_Ats.Application.Common.Helpers;
using Recsite_Ats.Application.Common.Interface.Businesslogic;
using Recsite_Ats.Application.Common.Interface.Repository;
using Recsite_Ats.Application.Common.Interface.Services;
using Recsite_Ats.Domain.API;
using Recsite_Ats.Domain.API.APIRequest;
using Recsite_Ats.Domain.DataTransferObject;
using Recsite_Ats.Domain.ViewModels;
using Recsite_Ats.Infrastructure.CustomException;
using System.Text.Json;
using static Recsite_Ats.Domain.Extend.AdminSetting;

namespace Recsite_Ats.Web.APIController;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
public class JobController : BaseAPIController
{
    public JobController(
        IUnitOfWork unitOfWork,
        IServices services,
        IBusinessLogic businessLogic,
        ClaimHelper claimHelper
        ) : base(unitOfWork, services, businessLogic, claimHelper)
    {

    }
    [Authorize]
    [HttpGet("get-all-job")]
    public async Task<BaseAPIResponse> GetAllJob()
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
            var request = new JobRequestDTO()
            {
                AccountId = accountId,
                TableName = Setting.Jobs.ToString(),
                Columns = Helper.GetColumnList(Setting.Jobs)
            };
            var result = await _logic.JobBusinessLogic.GetAllJobs(request);
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
                ActionName = nameof(GetAllJob),
                Message = response.RespDescription,
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [Authorize]
    [HttpGet("get-job")]
    public async Task<BaseAPIResponse> GetJob(int Id)
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
            var request = new JobRequestDTO()
            {
                AccountId = accountId,
                TableName = Setting.Jobs.ToString(),
                Columns = Helper.GetColumnList(Setting.Jobs),
                JobId = Id
            };
            var result = await _logic.JobBusinessLogic.GetJob(request);
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
                ActionName = nameof(GetJob),
                Message = response.RespDescription,
                RequestData = Id.ToString(),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [Authorize]
    [HttpPost("create-job")]
    public async Task<BaseAPIResponse> CreateJob(SettingCreateRequest request)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            if (!ModelState.IsValid)
            {
                var fieldErrors = ModelState
                .Where(ms => ms.Key.Contains("CustomFields") && ms.Value.Errors.Count > 0)
                .Select(ms => new ErrorViewModel
                {
                    Key = ms.Key,
                    Message = ms.Value.Errors.Select(e => e.ErrorMessage).FirstOrDefault()
                })
                .ToList();
                response.RespCode = "400";
                response.RespDescription = JsonSerializer.Serialize(fieldErrors);
                return response;
            }

            var claims = User.Claims.Where(x => x.Type == "AccountId").FirstOrDefault()?.Value;
            if (string.IsNullOrEmpty(claims))
            {
                response.RespCode = "404";
                response.RespDescription = "Invalid User! User is not Found";
                return response;
            }


            int accountId = int.Parse(claims);

            await _logic.JobBusinessLogic.CreateJob(request, accountId);

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
            string test = ex.InnerException.Message;
            response.RespCode = "500";
            return response;
        }
        finally
        {
            LoggingDTO log = new LoggingDTO()
            {
                ControllerName = nameof(JobController),
                ActionName = nameof(CreateJob),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [Authorize]
    [HttpPut("edit-job")]
    public async Task<BaseAPIResponse> EditJob(SettingCreateRequest request, int Id)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            if (!ModelState.IsValid)
            {
                response.RespCode = "400";
                response.RespDescription = "Validation failed";
                response.Data = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return response;
            }

            var claims = User.Claims.Where(x => x.Type == "AccountId").FirstOrDefault()?.Value;
            if (string.IsNullOrEmpty(claims))
            {
                response.RespCode = "404";
                response.RespDescription = "Invalid User! User is not Found";
                return response;
            }

            int accountId = int.Parse(claims);

            var JobData = new JobRequestDTO()
            {
                JobId = Id,
                AccountId = accountId,
            };

            await _logic.JobBusinessLogic.EditJob(request, JobData);

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
            response.RespCode = "500";
            response.RespDescription = string.IsNullOrEmpty(ex.InnerException?.Message) ? ex.Message : ex.InnerException.Message;
            return response;

        }
        finally
        {
            LoggingDTO log = new LoggingDTO()
            {
                ControllerName = nameof(JobController),
                ActionName = nameof(EditJob),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

}
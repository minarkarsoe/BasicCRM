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
public class CandidateController : BaseAPIController
{
    public CandidateController(
        IUnitOfWork unitOfWork,
        IServices services,
        IBusinessLogic businessLogic,
        ClaimHelper claimHelper
        ) : base(unitOfWork, services, businessLogic, claimHelper)
    {

    }

    [Authorize]
    [HttpGet("search-results")]
    public async Task<BaseAPIResponse> SearchResults()
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
            //var result = _logic.ContactBusinessLogic
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
                ControllerName = nameof(CompanyController),
                ActionName = nameof(SearchResults),
                Message = response.RespDescription,
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [Authorize]
    [HttpGet("get-candidate")]
    public async Task<BaseAPIResponse> GetCandidate(int id)
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
            var request = new CandidateRequestDTO()
            {
                AccountId = accountId,
                TableName = Setting.Candidates.ToString(),
                Columns = Helper.GetColumnList(Setting.Candidates),
                CandidateId = id
            };
            var result = await _logic.CandidateBusinessLogic.GetCandidate(request);
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
                ControllerName = nameof(CandidateController),
                ActionName = nameof(GetCandidate),
                Message = response.RespDescription,
                RequestData = id.ToString(),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [Authorize]
    [HttpPost("create-candidate")]
    public async Task<BaseAPIResponse> CreateCandidate(SettingCreateRequest request)
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

            await _logic.CandidateBusinessLogic.CreateCandidate(request, accountId);

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
                ControllerName = nameof(CandidateController),
                ActionName = nameof(CreateCandidate),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [Authorize]
    [HttpPut("edit-candidate")]
    public async Task<BaseAPIResponse> EditCandidate(SettingCreateRequest request, int Id)
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

            var CandidateData = new CandidateRequestDTO()
            {
                CandidateId = Id,
                AccountId = accountId,
            };

            await _logic.CandidateBusinessLogic.EditCandidate(request, CandidateData);

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
                ControllerName = nameof(CandidateController),
                ActionName = nameof(EditCandidate),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

}
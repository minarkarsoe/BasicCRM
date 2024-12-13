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
public class ContactController : BaseAPIController
{
    public ContactController(
        IUnitOfWork unitOfWork,
        IServices services,
        IBusinessLogic businessLogic,
        ClaimHelper claimHelper
        ) : base(unitOfWork, services, businessLogic, claimHelper)
    {
    }

    [Authorize]
    [HttpGet("search-results")]
    public async Task<BaseAPIResponse> SearchResults(int companyId)
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
            var result = await _logic.ContactBusinessLogic.SearchResults(accountId);
            if (result != null)
            {
                response.Data = result;
            }
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
                ControllerName = nameof(ContactController),
                ActionName = nameof(SearchResults),
                Message = response.RespDescription,
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [Authorize]
    [HttpGet("get-all-contact")]
    public async Task<BaseAPIResponse> GetAllContact()
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
            var request = new ContactDataRequestDTO()
            {
                AccountId = accountId,
                TableName = Setting.Jobs.ToString(),
                Columns = Helper.GetColumnList(Setting.Jobs)
            };

            var result = await _logic.ContactBusinessLogic.GetAllContacts(request);
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
                ControllerName = nameof(ContactController),
                ActionName = nameof(GetAllContact),
                Message = response.RespDescription,
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [Authorize]
    [HttpGet("get-contact")]
    public async Task<BaseAPIResponse> GetContact(int Id)
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
            var request = new ContactDataRequestDTO()
            {
                AccountId = accountId,
                TableName = Setting.Jobs.ToString(),
                Columns = Helper.GetColumnList(Setting.Jobs),
                ContactId = Id
            };
            var result = await _logic.ContactBusinessLogic.GetContact(request);
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
                ControllerName = nameof(ContactController),
                ActionName = nameof(GetContact),
                Message = response.RespDescription,
                RequestData = Id.ToString(),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [Authorize]
    [HttpPost("create-contact")]
    public async Task<BaseAPIResponse> CreateContact(SettingCreateRequest request)
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

            await _logic.ContactBusinessLogic.CreateContact(request, accountId);

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
                ControllerName = nameof(ContactController),
                ActionName = nameof(CreateContact),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [Authorize]
    [HttpPut("edit-contact")]
    public async Task<BaseAPIResponse> EditContact(SettingCreateRequest request, int Id)
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

            var ContactData = new ContactDataRequestDTO()
            {
                ContactId = Id,
                AccountId = accountId,
            };

            await _logic.ContactBusinessLogic.EditContact(request, ContactData);

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
                ControllerName = nameof(ContactController),
                ActionName = nameof(EditContact),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }
}

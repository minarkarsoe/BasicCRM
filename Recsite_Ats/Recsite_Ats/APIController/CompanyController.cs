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
public class CompanyController : BaseAPIController
{
    public CompanyController(
        IUnitOfWork unitOfWork,
        IServices services,
        IBusinessLogic businessLogic,
        ClaimHelper claimHelper
        ) : base(unitOfWork, services, businessLogic, claimHelper)
    {
    }

    [Authorize]
    [HttpGet("get-all-company")]
    public async Task<BaseAPIResponse> GetAllCompany()
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

            var request = new CompanyDataRequestDTO()
            {
                AccountId = accountId,
                TableName = Setting.Companies.ToString(),
                Columns = Helper.GetColumnList(Setting.Companies)
            };

            var result = await _logic.CompanyBusinessLogic.GetAllCompanies(request);
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
                ControllerName = nameof(CompanyController),
                ActionName = nameof(GetAllCompany),
                Message = response.RespDescription,
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [Authorize]
    [HttpGet("get-company")]
    public async Task<BaseAPIResponse> GetCompany(int id)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var claimTypes = _claimHelper.CheckAccountValidorNot();

            var request = new CompanyDataRequestDTO()
            {
                AccountId = claimTypes.AccountId,
                TableName = Setting.Companies.ToString(),
                Columns = Helper.GetColumnList(Setting.Companies),
                CompanyId = id
            };
            var result = await _logic.CompanyBusinessLogic.GetCompany(request);
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
                ControllerName = nameof(CompanyController),
                ActionName = nameof(GetCompany),
                Message = response.RespDescription,
                RequestData = id.ToString(),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [Authorize]
    [HttpPost("create-company")]
    public async Task<BaseAPIResponse> CreateCompany(SettingCreateRequest request)
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

            var claimTypes = _claimHelper.CheckAccountValidorNot();


            await _logic.CompanyBusinessLogic.CreateCompany(request, claimTypes);

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
                ActionName = nameof(CreateCompany),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [Authorize]
    [HttpPut("edit-company")]
    public async Task<BaseAPIResponse> EditCompany(SettingCreateRequest request, int Id)
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

            var CompanyData = new CompanyDataRequestDTO()
            {
                CompanyId = Id,
                AccountId = accountId,
            };

            await _logic.CompanyBusinessLogic.EditCompany(request, CompanyData);

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
                ControllerName = nameof(CompanyController),
                ActionName = nameof(EditCompany),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [Authorize]
    [HttpPost("creat-company-contact")]
    public async Task<BaseAPIResponse> CreateCompanyContact(CreateCompanyContact request)
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

            await _logic.CompanyBusinessLogic.CreateCompanyContact(request, accountId);

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
                ActionName = nameof(CreateCompanyContact),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [Authorize]
    [HttpPost("create-company-note")]
    public async Task<BaseAPIResponse> CreateCompanyNote(CreateCompanyNote request)
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

            await _logic.CompanyBusinessLogic.CreateCompanyNote(request, accountId);

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
                ActionName = nameof(CreateCompanyNote),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [Authorize]
    [HttpPost("create-company-document")]
    public async Task<BaseAPIResponse> CreateCompanyDocument(CreateCompanyDocument request)
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

            await _logic.CompanyBusinessLogic.CreateCompanyDocument(request, accountId);

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
                ActionName = nameof(CreateCompanyNote),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }
}
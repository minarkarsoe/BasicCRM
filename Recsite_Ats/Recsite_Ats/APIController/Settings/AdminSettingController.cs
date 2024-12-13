using Amazon.Runtime;
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
using Recsite_Ats.Domain.Extend;
using Recsite_Ats.Infrastructure.CustomException;
using Recsite_Ats.Web.APIController.OtherAPI;
using System.Text.Json;
using static Recsite_Ats.Domain.Extend.AdminSetting;

namespace Recsite_Ats.Web.APIController.Settings;
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
public class AdminSettingController : BaseAPIController
{
    public AdminSettingController(
        IUnitOfWork unitOfWork,
        IServices services,
        IBusinessLogic businessLogic,
        ClaimHelper claimHelper
        ) : base(unitOfWork, services, businessLogic, claimHelper)
    {
    }

    [HttpPost("get-setting")]
    [Authorize]
    public async Task<BaseAPIResponse> GetSetiing(AdminSettingRequest request)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            if (!AdminSetting.IsValidateEnumName<AdminSetting.Setting>(request.TableName))
            {
                response.RespCode = "404";
                response.RespDescription = "There is no type of this setting.";
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
            Setting setting = AdminSetting.ParseEnumName<AdminSetting.Setting>(request.TableName);
            var result = await _logic.AdminSettingBusinessLogic.GetSetting(request.TableName, accountId, Helper.GetColumnList(setting));
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
                ControllerName = nameof(AdminSettingController),
                ActionName = nameof(GetSetiing),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [HttpPut("update-order")]
    [Authorize]
    public async Task<BaseAPIResponse> UpdateOrder(UpdateOrderRequest request)
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

            if (request.CustomFields == null || !request.CustomFields.Any())
            {
                response.RespCode = "400";
                response.RespDescription = "Update data is empty.";
                return response;
            }

            var result = await _logic.AdminSettingBusinessLogic.UpdateSetting(request);
            if (result)
            {
                return response;
            }
            response.RespCode = "304";
            response.RespDescription = "Faild to update!";
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
            response.RespDescription = ex.Message;
            return response;
        }
        finally
        {
            LoggingDTO log = new LoggingDTO()
            {
                ControllerName = nameof(AdminSettingController),
                ActionName = nameof(UpdateOrder),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [HttpPost("create-section")]
    [Authorize]
    public async Task<BaseAPIResponse> CreateSection(CreateSectionRequest request)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            if (!AdminSetting.IsValidateEnumName<AdminSetting.Setting>(request.TableName))
            {
                response.RespCode = "404";
                response.RespDescription = "There is no type of this setting.";
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

            var result = await _logic.AdminSettingBusinessLogic.CreateSection(request, accountId);
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
            response.RespCode = "500";
            response.RespDescription = ex.Message;
            return response;
        }
        finally
        {
            LoggingDTO log = new LoggingDTO()
            {
                ControllerName = nameof(AdminSettingController),
                ActionName = nameof(UpdateOrder),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [HttpPut("update-section")]
    [Authorize]
    public async Task<BaseAPIResponse> UpdateSection(UpdateSectionRequest request)
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
            var result = await _logic.AdminSettingBusinessLogic.UpdateSection(request, accountId);
            if (!result)
            {
                response.RespCode = "404";
                response.RespDescription = "Can't update the section!";
                return response;
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
            response.RespCode = "500";
            response.RespDescription = ex.Message;
            return response;
        }
        finally
        {
            LoggingDTO log = new LoggingDTO()
            {
                ControllerName = nameof(AdminSettingController),
                ActionName = nameof(UpdateSection),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [HttpDelete("delete-section")]
    [Authorize]
    public async Task<BaseAPIResponse> DeleteSection(DeleteSectionRequest request)
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
            var result = await _logic.AdminSettingBusinessLogic.DeleteSection(request, accountId);
            if (!result)
            {
                response.RespCode = "404";
                response.RespDescription = "Can't delete the section.";
                return response;
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
            response.RespCode = "500";
            response.RespDescription = ex.Message;
            return response;
        }
        finally
        {
            LoggingDTO log = new LoggingDTO()
            {
                ControllerName = nameof(AdminSettingController),
                ActionName = nameof(DeleteSection),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }

    }

    [HttpPost("create-custom-field")]
    [Authorize]
    public async Task<BaseAPIResponse> CreateCustomField(CreateCustomFieldRequest request)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            if (!AdminSetting.IsValidateEnumName<AdminSetting.Setting>(request.TableName))
            {
                response.RespCode = "404";
                response.RespDescription = "There is no type of this setting.";
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
            var result = await _logic.AdminSettingBusinessLogic.CreateCustomField(request, accountId);
            if (result == null)
            {
                response.RespCode = "400";
                response.RespDescription = "Faild to create custom field";
                return response;
            }
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
            response.RespCode = "500";
            response.RespDescription = ex.Message;
            return response;
        }
        finally
        {
            LoggingDTO log = new LoggingDTO()
            {
                ControllerName = nameof(AdminSettingController),
                ActionName = nameof(CreateCustomField),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [HttpPut("update-custom-field")]
    [Authorize]
    public async Task<BaseAPIResponse> UpdateCustomField(UpdateCustomFieldRequest request)
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
            var result = await _logic.AdminSettingBusinessLogic.UpdateCustomField(request, accountId);
            if (result == null)
            {
                response.RespCode = "404";
                response.RespDescription = "Custom field not found.";
                return response;
            }
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
            response.RespCode = "500";
            response.RespDescription = ex.Message;
            return response;
        }
        finally
        {
            LoggingDTO log = new LoggingDTO()
            {
                ControllerName = nameof(AdminSettingController),
                ActionName = nameof(UpdateCustomField),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [HttpDelete("delete-custom-field")]
    [Authorize]
    public async Task<BaseAPIResponse> DeleteCustomField(DeleteCustomFieldRequest request)
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

            var result = await _logic.AdminSettingBusinessLogic.DeleteCustomField(request, accountId);
            if (!result)
            {
                response.RespCode = "404";
                response.RespDescription = "Custom field not found.";
                return response;
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
            response.RespCode = "500";
            response.RespDescription = ex.Message;
            return response;
        }
        finally
        {
            LoggingDTO log = new LoggingDTO()
            {
                ControllerName = nameof(AdminSettingController),
                ActionName = nameof(DeleteCustomField),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [Authorize]
    [HttpGet("get-country-data")]
    public async Task<BaseAPIResponse> GetCountry()
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

            var result = await _services.SettingService.GetCountryData();

            response.Data = result;
            return response;
        }
        catch (BusinessLogicException ex)
        {
            response.RespCode = ex.ResponseCode;
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
                ActionName = nameof(GetCountry),
                Message = response.RespDescription,
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [Authorize]
    [HttpPost("add-billing-information")]
    public async Task<BaseAPIResponse> AddBillingInformation(BillInformationDTO request)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var claimTypes = _claimHelper.CheckAccountValidorNot();

            await _services.SettingService.AddBillingInformation(request, claimTypes);

            return response;
        }
        catch (BusinessLogicException ex)
        {
            response.RespCode = ex.ResponseCode;
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
                ActionName = nameof(AddBillingInformation),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }
    [Authorize]
    [HttpGet("get-billing-information")]
    public async Task<BaseAPIResponse> GetBillingInformation()
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var claimTypes = _claimHelper.CheckAccountValidorNot();

            var result = await _services.SettingService.GetBillInformation(claimTypes);
            response.Data = result;
            return response;
        }
        catch (BusinessLogicException ex)
        {
            response.RespCode = ex.ResponseCode;
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
                ActionName = nameof(GetBillingInformation),
                Message = response.RespDescription,
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [Authorize]
    [HttpPost("add-payment-method")]
    public async Task<BaseAPIResponse> AddPaymentMehtod(PaymentMethodDTO request)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var claimTypes = _claimHelper.CheckAccountValidorNot();

            await _services.SettingService.AddPaymentMethod(request, claimTypes);

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
                ActionName = nameof(AddPaymentMehtod),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

}

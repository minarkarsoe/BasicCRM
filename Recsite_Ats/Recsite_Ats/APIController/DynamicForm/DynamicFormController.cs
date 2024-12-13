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
using System.Text.Json;
using static Recsite_Ats.Domain.Extend.AdminSetting;

namespace Recsite_Ats.Web.APIController.DynamicForm;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
public class DynamicFormController : BaseAPIController
{
    public DynamicFormController(
       IUnitOfWork unitOfWork,
       IServices services,
       IBusinessLogic businessLogic,
       ClaimHelper claimHelper
       ) : base(unitOfWork, services, businessLogic, claimHelper)
    {

    }

    [HttpPost("get-dynamic-form")]
    [Authorize]
    public async Task<BaseAPIResponse> GetDynamicForm(DynamicFormRequest request)
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
            var result = await _logic.DynamicFormBusinessLogic.GetDynamicFormData(request.TableName, accountId, Helper.GetColumnList(setting));
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
                ControllerName = nameof(DynamicFormController),
                ActionName = nameof(GetDynamicForm),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }
}

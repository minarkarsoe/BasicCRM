using Microsoft.AspNetCore.Mvc;
using Recsite_Ats.Application.Common.Helper;
using Recsite_Ats.Application.Common.Interface.Businesslogic;
using Recsite_Ats.Application.Common.Interface.Repository;
using Recsite_Ats.Application.Common.Interface.Services;
using Recsite_Ats.Domain.API;

namespace Recsite_Ats.Web.APIController;
[ApiController]
public class BaseAPIController : ControllerBase
{
    protected readonly IUnitOfWork _unitOfWork;
    protected readonly IServices _services;
    protected readonly IBusinessLogic _logic;
    protected readonly ClaimHelper _claimHelper;

    public IWebHostEnvironment _env
    {
        get
        {
            return (IWebHostEnvironment)(HttpContext.RequestServices.GetService(typeof(IWebHostEnvironment)));
        }
    }

    public bool IsDevelopment
    {
        get
        {
            return _env.EnvironmentName.ToLower() == "development";
        }
    }

    public BaseAPIController(IUnitOfWork unitOfWork, IServices services, IBusinessLogic businessLogic, ClaimHelper claimHelper)
    {
        _logic = businessLogic;
        _unitOfWork = unitOfWork;
        _services = services;
        _claimHelper = claimHelper;
    }

    internal BaseAPIResponse CreateResponse(object data, string status = "000", string message = "Success")
    {
        return new BaseAPIResponse(data, message, status, _env.EnvironmentName);
    }


    internal BaseAPIResponse CheckNull(object model, params string[] props)
    {
        foreach (string prop in props)
        {
            var property = model.GetType().GetProperty(prop);
            if (property != null)
            {
                if (string.IsNullOrEmpty((string)property.GetValue(model)))
                {
                    return CreateResponse(null, "113", $"{property.Name} is empty or wrong.");
                }
            }
            else
            {
                return CreateResponse(null, "113", $"{prop} is empty or wrong.");
            }
        }
        return CreateResponse(null, "000", $"Request model is valid.");
    }
}

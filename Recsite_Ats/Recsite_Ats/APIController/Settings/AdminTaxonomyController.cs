using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recsite_Ats.Application.Common.Helper;
using Recsite_Ats.Application.Common.Interface.Businesslogic;
using Recsite_Ats.Application.Common.Interface.Repository;
using Recsite_Ats.Application.Common.Interface.Services;
using Recsite_Ats.Domain.API;
using Recsite_Ats.Domain.DataTransferObject;
using Recsite_Ats.Infrastructure.CustomException;
using System.Text.Json;

namespace Recsite_Ats.Web.APIController.Settings;
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
[ApiController]
public class AdminTaxonomyController : BaseAPIController
{
    public AdminTaxonomyController(
       IUnitOfWork unitOfWork,
       IServices services,
       IBusinessLogic businessLogic,
       ClaimHelper claimHelper
       ) : base(unitOfWork, services, businessLogic, claimHelper)
    {
    }

    [HttpGet("get-note-types")]
    [Authorize]
    public async Task<BaseAPIResponse> GetNoteTypes()
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var claimTypes = _claimHelper.CheckAccountValidorNot();

            var result = await _logic.AdminTaxonomyBusinessLogic.GetNoteTypes();
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
                ControllerName = nameof(AdminTaxonomyController),
                ActionName = nameof(GetNoteTypes),
                Message = response.RespDescription,
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [HttpPost("update-note-types")]
    [Authorize]
    public async Task<BaseAPIResponse> UpdateNoteType(List<NoteTypeDto> request)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var claimTypes = _claimHelper.CheckAccountValidorNot();
            await _logic.AdminTaxonomyBusinessLogic.UpdateNoteType(request);
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
                ControllerName = nameof(AdminTaxonomyController),
                ActionName = nameof(GetNoteTypes),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [HttpDelete("delete-note-type")]
    [Authorize]
    public async Task<BaseAPIResponse> DeleteNoteType(NoteTypeDto request)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var claimTypes = _claimHelper.CheckAccountValidorNot();
            await _logic.AdminTaxonomyBusinessLogic.DeleteNoteType(request);
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
                ControllerName = nameof(AdminTaxonomyController),
                ActionName = nameof(DeleteNoteType),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [HttpGet("get-job-status")]
    [Authorize]
    public async Task<BaseAPIResponse> GetJobStatus()
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var claimTypes = _claimHelper.CheckAccountValidorNot();

            var result = await _logic.AdminTaxonomyBusinessLogic.GetJobStatus();
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
                ControllerName = nameof(AdminTaxonomyController),
                ActionName = nameof(GetJobStatus),
                Message = response.RespDescription,
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [HttpPost("update-job-status")]
    [Authorize]
    public async Task<BaseAPIResponse> UpdateJobStatus(List<JobStatusDto> request)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var claimTypes = _claimHelper.CheckAccountValidorNot();

            await _logic.AdminTaxonomyBusinessLogic.UpdateJobStatus(request);
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
                ControllerName = nameof(AdminTaxonomyController),
                ActionName = nameof(UpdateJobStatus),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [HttpDelete("delete-job-status")]
    [Authorize]
    public async Task<BaseAPIResponse> DeleteJobStatus(JobStatusDto request)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var claimTypes = _claimHelper.CheckAccountValidorNot();
            await _logic.AdminTaxonomyBusinessLogic.DeleteJobStatus(request);
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
                ControllerName = nameof(AdminTaxonomyController),
                ActionName = nameof(DeleteJobStatus),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [HttpGet("get-document-types")]
    [Authorize]
    public async Task<BaseAPIResponse> GetDocumentType()
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var claimTypes = _claimHelper.CheckAccountValidorNot();

            var result = await _logic.AdminTaxonomyBusinessLogic.GetDocumentTypes();
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
                ControllerName = nameof(AdminTaxonomyController),
                ActionName = nameof(GetDocumentType),
                Message = response.RespDescription,
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [HttpPost("update-document-types")]
    [Authorize]
    public async Task<BaseAPIResponse> UpdateDocumentTypes(List<DocumentTypeDto> request)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var claimTypes = _claimHelper.CheckAccountValidorNot();

            await _logic.AdminTaxonomyBusinessLogic.UpdateDocumentTypes(request);
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
                ControllerName = nameof(AdminTaxonomyController),
                ActionName = nameof(UpdateDocumentTypes),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [HttpDelete("delete-document-type")]
    [Authorize]
    public async Task<BaseAPIResponse> DeleteDcoumentType(DocumentTypeDto request)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var claimTypes = _claimHelper.CheckAccountValidorNot();
            await _logic.AdminTaxonomyBusinessLogic.DeletDocumentType(request);
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
                ControllerName = nameof(AdminTaxonomyController),
                ActionName = nameof(DeleteDcoumentType),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [HttpGet("get-contact-stages")]
    [Authorize]
    public async Task<BaseAPIResponse> GetContactStages()
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var claimTypes = _claimHelper.CheckAccountValidorNot();

            var result = await _logic.AdminTaxonomyBusinessLogic.GetContactStages();
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
                ControllerName = nameof(AdminTaxonomyController),
                ActionName = nameof(GetContactStages),
                Message = response.RespDescription,
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [HttpPost("update-contact-stages")]
    [Authorize]
    public async Task<BaseAPIResponse> UpdateContactStages(List<ContactStageDto> request)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var claimTypes = _claimHelper.CheckAccountValidorNot();

            await _logic.AdminTaxonomyBusinessLogic.UpdateContactStages(request);
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
                ControllerName = nameof(AdminTaxonomyController),
                ActionName = nameof(UpdateContactStages),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [HttpDelete("delete-contact-stage")]
    [Authorize]
    public async Task<BaseAPIResponse> DeleteContactStage(ContactStageDto request)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var claimTypes = _claimHelper.CheckAccountValidorNot();
            await _logic.AdminTaxonomyBusinessLogic.DeleteContactStage(request);
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
                ControllerName = nameof(AdminTaxonomyController),
                ActionName = nameof(DeleteContactStage),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [HttpGet("get-candidate-sources")]
    [Authorize]
    public async Task<BaseAPIResponse> GetCandidateSources()
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var claimTypes = _claimHelper.CheckAccountValidorNot();

            var result = await _logic.AdminTaxonomyBusinessLogic.GetCandidateSources();
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
                ControllerName = nameof(AdminTaxonomyController),
                ActionName = nameof(GetCandidateSources),
                Message = response.RespDescription,
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [HttpPost("update-candidate-sources")]
    [Authorize]
    public async Task<BaseAPIResponse> UpdateCandidateSources(List<CandidateSourceDto> request)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var claimTypes = _claimHelper.CheckAccountValidorNot();

            await _logic.AdminTaxonomyBusinessLogic.UpdateCandidateSources(request);
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
                ControllerName = nameof(AdminTaxonomyController),
                ActionName = nameof(UpdateCandidateSources),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [HttpDelete("delete-candidate-source")]
    [Authorize]
    public async Task<BaseAPIResponse> DeleteCandidateSource(CandidateSourceDto request)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var claimTypes = _claimHelper.CheckAccountValidorNot();
            await _logic.AdminTaxonomyBusinessLogic.DeletCandidateSource(request);
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
                ControllerName = nameof(AdminTaxonomyController),
                ActionName = nameof(DeleteCandidateSource),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [HttpGet("get-job-categories")]
    [Authorize]
    public async Task<BaseAPIResponse> GetJobCategories()
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var claimTypes = _claimHelper.CheckAccountValidorNot();

            var result = await _logic.AdminTaxonomyBusinessLogic.GetJobCategories();
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
                ControllerName = nameof(AdminTaxonomyController),
                ActionName = nameof(GetJobCategories),
                Message = response.RespDescription,
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [HttpPost("update-job-categories")]
    [Authorize]
    public async Task<BaseAPIResponse> UpdateJobCategories(List<JobCategoryDto> request)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var claimTypes = _claimHelper.CheckAccountValidorNot();

            await _logic.AdminTaxonomyBusinessLogic.UpdateJobCategories(request);
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
                ControllerName = nameof(AdminTaxonomyController),
                ActionName = nameof(UpdateJobCategories),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [HttpDelete("delete-job-category")]
    [Authorize]
    public async Task<BaseAPIResponse> DeleteJobCategory(JobCategoryDto request)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var claimTypes = _claimHelper.CheckAccountValidorNot();
            await _logic.AdminTaxonomyBusinessLogic.DeleteJobCategory(request);
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
                ControllerName = nameof(AdminTaxonomyController),
                ActionName = nameof(DeleteJobCategory),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [HttpDelete("delete-job-sub-category")]
    [Authorize]
    public async Task<BaseAPIResponse> DeleteJobSubCategory(JobSubCategoryDto request)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var claimTypes = _claimHelper.CheckAccountValidorNot();
            await _logic.AdminTaxonomyBusinessLogic.DeleteJobSubCategory(request);
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
                ControllerName = nameof(AdminTaxonomyController),
                ActionName = nameof(DeleteJobSubCategory),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [HttpGet("get-job-locations")]
    [Authorize]
    public async Task<BaseAPIResponse> GetJobLocations()
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var claimTypes = _claimHelper.CheckAccountValidorNot();

            var result = await _logic.AdminTaxonomyBusinessLogic.GetJoblocations();
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
                ControllerName = nameof(AdminTaxonomyController),
                ActionName = nameof(GetJobLocations),
                Message = response.RespDescription,
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [HttpPost("update-job-locations")]
    [Authorize]
    public async Task<BaseAPIResponse> UpdateJobLocations(List<JobLocationDto> request)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var claimTypes = _claimHelper.CheckAccountValidorNot();

            await _logic.AdminTaxonomyBusinessLogic.UpdateJobLocations(request);
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
                ControllerName = nameof(AdminTaxonomyController),
                ActionName = nameof(UpdateJobLocations),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [HttpDelete("delete-job-location")]
    [Authorize]
    public async Task<BaseAPIResponse> DeleteJobLocation(JobLocationDto request)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var claimTypes = _claimHelper.CheckAccountValidorNot();
            await _logic.AdminTaxonomyBusinessLogic.DeleteJobLocation(request);
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
                ControllerName = nameof(AdminTaxonomyController),
                ActionName = nameof(DeleteJobLocation),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [HttpDelete("delete-job-sub-location")]
    [Authorize]
    public async Task<BaseAPIResponse> DeleteJobSubLocation(JobSubLocationDto request)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var claimTypes = _claimHelper.CheckAccountValidorNot();
            await _logic.AdminTaxonomyBusinessLogic.DeleteJobSubLocation(request);
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
                ControllerName = nameof(AdminTaxonomyController),
                ActionName = nameof(DeleteJobSubLocation),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }
}

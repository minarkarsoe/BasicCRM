using Recsite_Ats.Domain.API.APIRequest;
using Recsite_Ats.Domain.API.APIResponse;
using Recsite_Ats.Domain.DataTransferObject;

namespace Recsite_Ats.Application.Common.Interface.Businesslogic;
public interface IJobBusinessLogic
{
    Task<JobResponseDTO> GetAllJobs(JobRequestDTO requestDTO);
    Task<SettingDataAPIResponse> GetJob(JobRequestDTO requestDTO);
    Task CreateJob(SettingCreateRequest requestDTO, int? accountId);
    Task EditJob(SettingCreateRequest requestDTO, JobRequestDTO jobData);
    Task DeleteJob(CompanyDataRequestDTO requestDTO);
}

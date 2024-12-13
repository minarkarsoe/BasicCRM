using Recsite_Ats.Domain.API.APIRequest;
using Recsite_Ats.Domain.API.APIResponse;
using Recsite_Ats.Domain.DataTransferObject;

namespace Recsite_Ats.Application.Common.Interface.Businesslogic;
public interface ICandidateBusinesslogic
{
    Task<CandidateResponseDTO> GetAllCandidates(CandidateRequestDTO requestDTO);
    Task<SettingDataAPIResponse> GetCandidate(CandidateRequestDTO requestDTO);
    Task CreateCandidate(SettingCreateRequest requestDTO, int? accountId);
    Task EditCandidate(SettingCreateRequest requestDTO, CandidateRequestDTO companyData);
    Task DeleteCandidate(CompanyDataRequestDTO requestDTO);
    Task<List<SearchDataResponse>> SearchResults(int accountId);
}

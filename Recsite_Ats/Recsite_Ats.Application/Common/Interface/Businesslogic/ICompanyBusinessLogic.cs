using Recsite_Ats.Domain.API.APIRequest;
using Recsite_Ats.Domain.API.APIResponse;
using Recsite_Ats.Domain.DataTransferObject;
using Recsite_Ats.Domain.DataTransferObject.UserDTO;

namespace Recsite_Ats.Application.Common.Interface.Businesslogic;
public interface ICompanyBusinessLogic
{
    Task<CompanyDataResponseDTO> GetAllCompanies(CompanyDataRequestDTO requestDTO);
    Task<CompanyDetailDataResponse> GetCompany(CompanyDataRequestDTO requestDTO);
    Task CreateCompany(SettingCreateRequest requestDTO, ClaimTypesDto claimTypes);
    Task EditCompany(SettingCreateRequest requestDTO, CompanyDataRequestDTO companyData);
    Task DeleteCompany(CompanyDataRequestDTO requestDTO);
    Task CreateCompanyContact(CreateCompanyContact requestDTO, int accountId);
    Task CreateCompanyNote(CreateCompanyNote requestDTO, int accountId);
    Task CreateCompanyDocument(CreateCompanyDocument requestDTO, int accountId);
}

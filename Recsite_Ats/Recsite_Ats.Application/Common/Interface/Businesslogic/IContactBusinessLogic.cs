using Recsite_Ats.Domain.API.APIRequest;
using Recsite_Ats.Domain.API.APIResponse;
using Recsite_Ats.Domain.DataTransferObject;

namespace Recsite_Ats.Application.Common.Interface.Businesslogic;
public interface IContactBusinessLogic
{
    Task<ContactDataResponseDTO> GetAllContacts(ContactDataRequestDTO requestDTO);
    Task<SettingDataAPIResponse> GetContact(ContactDataRequestDTO requestDTO);
    Task CreateContact(SettingCreateRequest requestDTO, int? accountId);
    Task EditContact(SettingCreateRequest requestDTO, ContactDataRequestDTO companyData);
    Task DeleteContact(ContactDataRequestDTO requestDTO);
    Task<SearchDataResponse> SearchResults(int accountId);
}

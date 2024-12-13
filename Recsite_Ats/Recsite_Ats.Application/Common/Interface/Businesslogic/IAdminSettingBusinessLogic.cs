using Recsite_Ats.Domain.API.APIRequest;
using Recsite_Ats.Domain.DataTransferObject;
using Recsite_Ats.Domain.Entites;

namespace Recsite_Ats.Application.Common.Interface.Businesslogic;
public interface IAdminSettingBusinessLogic
{
    Task<SectionLayoutDTO> GetSetting(string tableName, int? accountId, string columnNames);
    Task<SectionLayout> CreateSection(CreateSectionRequest request, int accountId);
    Task<bool> UpdateSetting(UpdateOrderRequest request);
    Task<bool> UpdateSection(UpdateSectionRequest request, int accountId);
    Task<bool> DeleteSection(DeleteSectionRequest request, int accountId);
    Task<CustomField> CreateCustomField(CreateCustomFieldRequest request, int accountId);
    Task<CustomField> UpdateCustomField(UpdateCustomFieldRequest request, int accountId);
    Task<bool> DeleteCustomField(DeleteCustomFieldRequest request, int accountId);
}

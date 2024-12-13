using Recsite_Ats.Domain.API.APIResponse;
using Recsite_Ats.Domain.DataTransferObject;
using Recsite_Ats.Domain.DataTransferObject.UserDTO;
using Recsite_Ats.Domain.Entites;

namespace Recsite_Ats.Application.Common.Interface.Services;
public interface ISettingService
{
    Task<SectionLayoutDTO> GetFieldDetailsAsync(string tableName, int? accountId, string columnNames);
    Task<FieldLayout?> CreateFieldLayout(FieldLayout fieldLayout);
    Task<List<CountryDataResponse>> GetCountryData();
    Task AddBillingInformation(BillInformationDTO billingInformation, ClaimTypesDto? claimTypes);
    Task<BillInformationDTO> GetBillInformation(ClaimTypesDto? claimTypes);
    Task AddPaymentMethod(PaymentMethodDTO paymentMethod, ClaimTypesDto? claimTypes);
}

using Recsite_Ats.Domain.DataTransferObject;

namespace Recsite_Ats.Application.Common.Interface.Businesslogic;
public interface IDynamicFormBusinessLogic
{
    Task<DynamicFormLayoutDTO> GetDynamicFormData(string tableName, int? accountId, string columnNames);
}

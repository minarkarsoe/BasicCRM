using Recsite_Ats.Domain.Entites;

namespace Recsite_Ats.Application.Common.Interface.Services;
public interface IAccountService
{
    Task<Account?> InsertAndGetAccount(Account account);
}

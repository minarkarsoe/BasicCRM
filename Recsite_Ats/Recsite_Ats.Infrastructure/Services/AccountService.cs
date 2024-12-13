using Recsite_Ats.Application.Common.Interface.Repository;
using Recsite_Ats.Application.Common.Interface.Services;
using Recsite_Ats.Domain.Entites;

namespace Recsite_Ats.Infrastructure.Services;
public class AccountService : IAccountService
{
    private readonly IUnitOfWork _unitOfWork;

    public AccountService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Account?> InsertAndGetAccount(Account account)
    {
        var test = await _unitOfWork.Account.Add(account);
        await _unitOfWork.Save();
        return test;
    }
}

using Microsoft.AspNetCore.Http;
using Recsite_Ats.Domain.DataTransferObject.UserDTO;

namespace Recsite_Ats.Application.Common.Helper;
public class ClaimHelper
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public ClaimHelper(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public string GetUserId() => _httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value;
    public string GetAccountId() => _httpContextAccessor.HttpContext.User.FindFirst("AccountId")?.Value;

    public ClaimTypesDto CheckAccountValidorNot()
    {
        var userIdParsed = int.TryParse(GetUserId(), out int userId);
        var accountIdParsed = int.TryParse(GetAccountId(), out int accountId);

        if (!userIdParsed || !accountIdParsed)
        {
            throw new InvalidOperationException("Invalid User! User is not Found");
        }

        var claimsDto = new ClaimTypesDto()
        {
            UserId = userId,
            AccountId = accountId,
        };
        return claimsDto;
    }
}

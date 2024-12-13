using Recsite_Ats.Domain.Entites;

namespace Recsite_Ats.Application.Common.Interface.Services;

public interface IUserService
{
    Task<IEnumerable<UserSubscription>> GetUserByIdAsync(int userId);
    Task UpgradeSubscriptionAsync(int userId, int tier, DateTime durationInDays);
    Task<bool> IsUserInTierAsync(int userId, int tier);
}

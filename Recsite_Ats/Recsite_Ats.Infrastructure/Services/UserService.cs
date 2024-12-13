using Recsite_Ats.Application.Common.Interface.Repository;
namespace Recsite_Ats.Infrastructure.Services;
public class UserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /*public async Task<IEnumerable<UserSubscription>> GetUserByIdAsync(int userId)
    {
        return await _unitOfWork.UserSubscription.GetAll(u => u.UserId == userId, includeProperties: "User,Subscription");
    }

    public async Task UpgradeSubscriptionAsync(int userId, int tier, DateTime durationInDays)
    {
        var user = await _unitOfWork.User.Get(u => u.Id == userId);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        //user.SubscriptionTier = tier;
        //user.SubscriptionExpiryDate = DateTime.UtcNow.AddDays(durationInDays.Day - DateTime.Now.Day);

        _unitOfWork.User.Update(user);
        _unitOfWork.Save();
    }

    public async Task<bool> IsUserInTierAsync(int userId, int tier)
    {
        var user = _unitOfWork.User.Get(u => u.Id == userId);

        if (user == null)
        {
            throw new Exception("User not found");
        }

        return true *//*user.SubscriptionTier == tier && user.SubscriptionExpiryDate > DateTime.UtcNow*//*;
    }*/
}

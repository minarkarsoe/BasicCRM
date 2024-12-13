using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Recsite_Ats.Application.Common.Interface.Services;
using Recsite_Ats.Domain.Entites;
using Recsite_Ats.Domain.Extend;
using System.Security.Claims;

namespace Recsite_Ats.Infrastructure.Data;

public class CustomClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>
{
    private readonly IServices _services;
    public CustomClaimsPrincipalFactory(
        IServices services,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, roleManager, optionsAccessor)
    {
        _services = services;
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);
        var userSubscription = await _services.UserService.GetUserByIdAsync(user.Id);
        var tier = userSubscription.Select(x => x.Subscription.Tier).ToList();
        string Premium = "0";
        string Basic = tier.Contains((int)SubscriptionLvl.SubscriptionLevel.Basic) ? "1" : "0";
        if (tier.Contains((int)SubscriptionLvl.SubscriptionLevel.Premium))
        {
            Basic = "1";
            Premium = "1";
        }

        identity.AddClaim(new Claim("Free", "1"));
        identity.AddClaim(new Claim("Premium", Premium));
        identity.AddClaim(new Claim("Basic", Basic));
        return identity;
    }
}


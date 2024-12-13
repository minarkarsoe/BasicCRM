using Microsoft.AspNetCore.Identity;
using Recsite_Ats.Application.Common.Interface.Services;
using Recsite_Ats.Domain.DataTransferObject.UserDTO;
using Recsite_Ats.Domain.Entites;
using Recsite_Ats.Infrastructure.CustomException;

namespace Recsite_Ats.Infrastructure.Services;
public class AdminService : IAdminService
{
    private readonly UserManager<ApplicationUser> _userManager;
    public AdminService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task RegisterRecruiterAsync(RegisterRecruiterDto model, string adminId)
    {
        var adminUser = await _userManager.FindByIdAsync(adminId);
        if (adminUser == null) throw new BusinessLogicException("404", "Admin not found");

        var recruiter = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            PhoneNumber = model.Phone,
            AccountId = model.AccountId,
        };

        var result = await _userManager.CreateAsync(recruiter, model.Password);
        if (!result.Succeeded) throw new BusinessLogicException("400", string.Join(", ", result.Errors.Select(e => e.Description)));

        await _userManager.AddToRoleAsync(recruiter, "Recruiter");
    }
}

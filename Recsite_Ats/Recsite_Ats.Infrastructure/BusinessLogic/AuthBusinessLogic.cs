using Microsoft.AspNetCore.Identity;
using Recsite_Ats.Application.Common.Interface.Businesslogic;
using Recsite_Ats.Application.Common.Interface.Repository;
using Recsite_Ats.Application.Common.Interface.Services;
using Recsite_Ats.Application.Common.Utility;
using Recsite_Ats.Domain.API.APIRequest;
using Recsite_Ats.Domain.API.APIResponse;
using Recsite_Ats.Domain.DataTransferObject.UserDTO;
using Recsite_Ats.Domain.Entites;
using Recsite_Ats.Infrastructure.CustomException;

namespace Recsite_Ats.Infrastructure.BusinessLogic;
public class AuthBusinessLogic : IAuthBusinessLogic
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IServices _services;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public AuthBusinessLogic(
        IUnitOfWork unitOfWork,
        IServices services,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _unitOfWork = unitOfWork;
        _services = services;
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
    }
    public async Task<RegisterResponse> UserRegister(RegisterRequest request)
    {
        IdentityResult? result = null;
        ApplicationUser? existingUser = await _unitOfWork.User.Get(x => x.Email == request.Email);
        if (existingUser != null)
        {
            throw new BusinessLogicException("409", "User is already existed.");
        }

        // 1.2 We need to write code to insert the account into the database

        Account account = new Account()
        {
            PrimaryCountry = "UK",
            CompanyName = request.CompanyName,
            ContactFirstName = request.FirstName,
            ContactLastName = request.LastName,
            ContactEmail = request.Email,
            ContactPhone = request.Phone,
        };

        var newaccount = await _services.AccountService.InsertAndGetAccount(account);

        Seat seat = new Seat()
        {
            TotalSeats = 10,
            UsedSeats = 0,
            SeatRenewelAmount = 0,
            SeatRenewelDate = DateTime.Now.AddDays(30),
            AccountId = newaccount.Id,
        };

        //2. We then need to create an admin user and assign them to the account
        ApplicationUser user = new()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.Phone,
            PhoneNumberConfirmed = true,
            AccountId = newaccount.Id,
            NormalizedEmail = request.Email.ToUpper(),
            EmailConfirmed = true,
            UserName = request.Email,
            CreatedBy = 1
        };

        result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            if (!string.IsNullOrEmpty(request.Role))
            {

                await _userManager.AddToRoleAsync(user, request.Role);
            }
            else
            {
                await _userManager.AddToRoleAsync(user, SD.Role_Recruiter);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            var response = new RegisterResponse
            {
                CompanyName = request.CompanyName,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
            };
            return response;
        }
        throw new BusinessLogicException("400", "Failed to register your account.");
    }


    public async Task<RefreshTokenResponseDto> Login(ApplicationUser user)
    {
        RefreshTokenResponseDto result = new RefreshTokenResponseDto();
        result.Token = await _services.TokenService.GenerateJwtToken(user);
        result.RefreshToken = await _services.TokenService.GenerateRefreshToken(user, result.Token);
        result.UserData = new
        {
            TwoFactorRequired = await _userManager.GetTwoFactorEnabledAsync(user),
            FullName = user?.UserName,
            UserEmail = user?.Email,
            CompanyName = user?.Account.CompanyName
        };
        return result;

    }

    public async Task<List<UserResponseDto>> GetUsers(int accountId)
    {
        var response = new List<UserResponseDto>();

        var getUser = await _unitOfWork.User.GetAll(x => x.AccountId == accountId, "Account");
        foreach (var item in getUser)
        {
            var user = new UserResponseDto();
            var roleNames = await _userManager.GetRolesAsync(item);
            foreach (var roleName in roleNames)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    user.RoleId = role.Id;
                    user.RoleName = roleName;
                }
            }
            user.AccountId = accountId;
            user.UserId = item.Id;
            user.Email = item.Email;
            user.FirstName = item.FirstName;
            user.LastName = item.LastName;
            user.PhoneNumber = item.PhoneNumber;
            user.CreatedDate = item.CreatedDate;
            response.Add(user);
        }
        return response;
    }

    public async Task<RegisterResponse> RegisterRecruiter(RegisterRecruiterDto request, int accountId)
    {
        IdentityResult? result = null;
        ApplicationUser? existingUser = await _unitOfWork.User.Get(x => x.Email == request.Email);
        if (existingUser != null)
        {
            throw new BusinessLogicException("409", "User is already existed.");
        }
        ApplicationUser user = new()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.Phone,
            PhoneNumberConfirmed = true,
            AccountId = accountId,
            NormalizedEmail = request.Email.ToUpper(),
            EmailConfirmed = true,
            UserName = request.Email,
            CreatedBy = accountId,
        };
        result = await _userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, SD.Role_Recruiter);
            var seat = await _unitOfWork.Seat.Get(x => x.AccountId == accountId);
            if (seat != null)
            {
                seat.UsedSeats += 1;
                await _unitOfWork.Seat.Update(seat);
                await _unitOfWork.Save();
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            var response = new RegisterResponse
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
            };
            return response;
        }
        else
        {
            throw new BusinessLogicException("400", "Failed to register your account.");
        }
    }

    public async Task<RegisterResponse> EditRecruiter(EidtRecruiterDto request, int accountId)
    {
        ApplicationUser? user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user != null)
        {
            user.Email = request.Email ?? user.Email;
            user.UserName = request.Email ?? user.UserName;
            user.FirstName = request.FirstName ?? user.FirstName;
            user.LastName = request.LastName ?? user.LastName;
            user.PhoneNumber = request.Phone ?? user.PhoneNumber;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                var response = new RegisterResponse
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                };
                return response;
            }
            else
            {
                throw new BusinessLogicException("400", "Failed to update recruiter");
            }
        }
        else
        {
            throw new BusinessLogicException("404", "User is not Found");
        }
    }
}

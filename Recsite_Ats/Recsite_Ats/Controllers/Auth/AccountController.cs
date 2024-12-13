/*using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Recsite_Ats.Application.Common.Interface.Repository;
using Recsite_Ats.Application.Common.Interface.Services;
using Recsite_Ats.Application.Common.Utility;
using Recsite_Ats.Domain.Entites;
using Recsite_Ats.Domain.ViewModels;
using static Recsite_Ats.Domain.Extend.SubscriptionLvl;

namespace Recsite_Ats.Web.Controllers.Auth;
public class AccountController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IServices _services;

    public AccountController(
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

    #region Login & Register (2FA inculde)
    public IActionResult Login(string returnUrl = null)
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home"); // Redirect to home or any other page
        }
        returnUrl ??= Url.Content("~/");

        LoginVM loginVM = new LoginVM()
        {
            RedirectUrl = returnUrl
        };

        return View(loginVM);
    }

    public IActionResult Register(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        if (!_roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
        {
            var admin_role = new ApplicationRole()
            {
                RoleName = SD.Role_Admin,
                Name = SD.Role_Admin,
            };
            var user_role = new ApplicationRole()
            {
                RoleName = SD.Role_Customer,
                Name = SD.Role_Customer,
            };
            _roleManager.CreateAsync(admin_role).Wait();
            _roleManager.CreateAsync(user_role).Wait();
        }
        RegisterVM registerVM = new RegisterVM()
        {
            RoleList = _roleManager.Roles.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Name
            }),
            RedirectUrl = returnUrl
        };

        return View(registerVM);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterVM registerVM)
    {
        try
        {
            if (ModelState.IsValid)
            {

                //1. We need to create an account object

                // 1.1 We need to check to see if an account exists related to that email (return error if exists)

                ApplicationUser? existingUser = await _unitOfWork.User.Get(x => x.Email == registerVM.Email);
                if (existingUser != null)
                {
                    return View();
                }

                // 1.2 We need to write code to insert the account into the database

                Account account = new Account()
                {
                    PrimaryCountry = "UK",
                    CompanyName = registerVM.CompanyName,
                    ContactFirstName = registerVM.FirstName,
                    ContactLastName = registerVM.LastName,
                    ContactEmail = registerVM.Email,
                    ContactPhone = registerVM.Phone,
                    Seats = 14,
                    SeatCustomPrice = 20,
                    SeatRenewalDate = DateTime.Now.AddDays(30)
                };

                var newaccount = await _services.AccountService.InsertAndGetAccount(account);

                //2. We then need to create an admin user and assign them to the account
                ApplicationUser user = new()
                {
                    FirstName = registerVM.FirstName,
                    LastName = registerVM.LastName,
                    Email = registerVM.Email,
                    Phone = registerVM.Phone,
                    AccountId = newaccount.Id,
                    NormalizedEmail = registerVM.Email.ToUpper(),
                    IsAssignedSeat = true,
                    EmailConfirmed = true,
                    UserName = registerVM.Email,
                    CreatedBy = 1
                };

                var result = await _userManager.CreateAsync(user, registerVM.Password);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(registerVM.Role))
                    {
                        await _userManager.AddToRoleAsync(user, registerVM.Role);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, SD.Role_Customer);
                    }



                    await _signInManager.SignInAsync(user, isPersistent: false);

                    if (string.IsNullOrEmpty(registerVM.RedirectUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return LocalRedirect(registerVM.RedirectUrl);
                    }
                }

                foreach (var error in result.Errors)
                {

                    ModelState.AddModelError("", error.Description);
                }

            }

        }
        catch (Exception ex)
        {
            //_unitOfWork.Logging(this, ex.Message);
            return BadRequest(ex.Message);
        }

        List<SubscriptionLevel> subscriptionLevelsEnumerable = Enum.GetValues(typeof(SubscriptionLevel))
                                                                   .Cast<SubscriptionLevel>()
                                                                   .ToList();

        registerVM.SubscriptionList = subscriptionLevelsEnumerable.Select(x => new SelectListItem
        {
            Text = x.ToString(),
            Value = x.ToString()
        });

        registerVM.RoleList = _roleManager.Roles.Select(x => new SelectListItem
        {
            Text = x.Name,
            Value = x.Name
        });

        return View(registerVM);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginVM loginVM, string returnUrl = null)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(loginVM.Email, loginVM.Password, loginVM.RemeberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                if (string.IsNullOrEmpty(loginVM.RedirectUrl))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return LocalRedirect(loginVM.RedirectUrl);
                }
            }
            else if (result.RequiresTwoFactor)
            {
                return RedirectToAction(nameof(LoginWith2fa), new { returnUrl, loginVM.RemeberMe });
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt");
            }
        }

        return View(loginVM);
    }

    [HttpGet]
    public async Task<IActionResult> LoginWith2fa(bool rememberMe, string returnUrl = null)
    {
        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        if (user == null)
        {
            throw new InvalidOperationException($"Unable to load two-factor authentication user.");
        }
        if (string.IsNullOrEmpty(returnUrl))
        {
            return RedirectToAction("Index", "Home");
        }
        var model = new LoginWith2faViewModel { RememberMe = rememberMe };
        ViewData["ReturnUrl"] = returnUrl;
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginWith2fa(LoginWith2faViewModel model, bool rememberMe, string returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        if (user == null)
        {
            throw new InvalidOperationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

        var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, model.RememberMachine);

        if (result.Succeeded)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction("Index", "Home");
            }
            return LocalRedirect(returnUrl);
        }
        else if (result.IsLockedOut)
        {
            ModelState.AddModelError(string.Empty, "User account locked out.");
            return View(model);
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
            return View(model);
        }
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "Account");
    }

    #endregion

    #region Forgot Password

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Scheme);
            //await _services.EmailSender.SendPasswordResetEmailAsync(model.Email, user.UserName, callbackUrl);
            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }

        return View(model);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult ForgotPasswordConfirmation()
    {
        return View();
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult ResetPassword(string code = null)
    {
        return code == null ? View("Error") : View(new ResetPasswordViewModel { Code = code });
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return RedirectToAction(nameof(ResetPasswordConfirmation));
        }
        var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
        if (result.Succeeded)
        {
            return RedirectToAction(nameof(ResetPasswordConfirmation));
        }
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
        return View();
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult ResetPasswordConfirmation()
    {
        return View();
    }

    #endregion

    #region 2FA Recover

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Recover()
    {
        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        if (user == null)
        {
            return BadRequest("Unable to load two-factor authentication user.");
        }

        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Recover(RecoveryRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "User not found");
            return View(model);
        }

        if (model.RecoveryType == "Email")
        {
            var code = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
            //await _services.EmailSender.Send2FARecovryEmailAsync(user.Email, code);
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Currently we only allow recovery by email.");
        }
        return RedirectToAction("RecoveryCodeSent");
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult RecoveryCodeSent()
    {
        return View();
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Reset2FA()
    {
        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        if (user == null)
        {
            return BadRequest("Unable to load two-factor authentication user.");
        }
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reset2FA(Reset2FAViewModel model)
    {

        if (string.IsNullOrEmpty(model.Code))
        {
            ModelState.AddModelError(string.Empty, "Code is required");
            return View(model);
        }

        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        if (user == null)
        {
            return RedirectToAction("Error");
        }

        var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, "Email", model.Code); // Use the appropriate provider
        if (isValid)
        {
            // Reset 2FA settings and allow user to set up a new authenticator app
            await _userManager.SetTwoFactorEnabledAsync(user, false);
            // Redirect to the setup page or another page
            return RedirectToAction("EnableAuthenticator", "TwoFactorAuth");
        }

        ModelState.AddModelError(string.Empty, "Invalid code");
        return View(model);
    }

    #endregion
    public IActionResult AccessDenied()
    {
        return View();
    }

}

*/
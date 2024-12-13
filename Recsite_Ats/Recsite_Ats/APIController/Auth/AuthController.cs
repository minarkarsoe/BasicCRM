using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using Recsite_Ats.Application.Common.Helper;
using Recsite_Ats.Application.Common.Helpers;
using Recsite_Ats.Application.Common.Interface.Businesslogic;
using Recsite_Ats.Application.Common.Interface.Repository;
using Recsite_Ats.Application.Common.Interface.Services;
using Recsite_Ats.Domain.API;
using Recsite_Ats.Domain.API.APIRequest;
using Recsite_Ats.Domain.API.APIResponse;
using Recsite_Ats.Domain.DataTransferObject;
using Recsite_Ats.Domain.DataTransferObject.UserDTO;
using Recsite_Ats.Domain.Entites;
using Recsite_Ats.Infrastructure.CustomException;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using RegisterRequest = Recsite_Ats.Domain.API.APIRequest.RegisterRequest;

namespace Recsite_Ats.Web.APIController.Auth;
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
[ApiController]
public class AuthController : BaseAPIController
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IServices _services;

    public AuthController(
        IUnitOfWork unitOfWork,
        IServices services,
        IConfiguration configuration,
        IBusinessLogic businessLogic,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        SignInManager<ApplicationUser> signInManager,
        ClaimHelper claimHelper)
        : base(unitOfWork, services, businessLogic, claimHelper)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _services = services;
    }

    /* [HttpPost("login")]
     public async Task<BaseAPIResponse> Login(LoginRequest request)
     {
         BaseAPIResponse response = CreateResponse(null);
         try
         {
             string token = string.Empty;
             var user = await _userManager.FindByNameAsync(request.Username);
             if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
             {
                 object userData = new
                 {
                     TwoFactorRequired = true,
                     FullName = user?.UserName,
                     UserEmail = user?.Email
                 };
                 var result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, request.RememberMe, lockoutOnFailure: false);
                 if (result.RequiresTwoFactor)
                 {
                     token = GenerateJwtToken(user, true);
                     response.Data = new
                     {
                         Token = token,
                         UserInfo = userData
                     };
                     return response;

                 }
                 token = GenerateJwtToken(user);
                 response.Data = new
                 {
                     Token = token,
                     UserInfo = userData
                 };
                 return response;
             }
             response.RespCode = "404";
             response.RespDescription = "User is not found!";
             return response;
         }
         catch (Exception ex)
         {
             response.RespCode = "500";
             response.RespDescription = ex.Message;
             return response;
         }
         finally
         {
             LoggingDTO log = new LoggingDTO()
             {
                 ControllerName = nameof(AuthController),
                 ActionName = nameof(Login),
                 Message = response.RespDescription,
                 RequestData = JsonSerializer.Serialize(request),
                 ResponseData = JsonSerializer.Serialize(response)
             };
             await _unitOfWork.Logging(log);
         }
     }*/

    [Authorize]
    [HttpGet("claim")]
    public async Task<BaseAPIResponse> GetSecureData()
    {
        var claimValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                     ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (string.IsNullOrEmpty(claimValue))
        {
            return CreateResponse(null, "401", "Claim not found.");
        }
        BaseAPIResponse response = CreateResponse(null);

        response.Data = new { claimvalue = claimValue };

        // Return the claim value
        return response;
    }

    [AllowAnonymous]
    [HttpPost("register")]

    public async Task<BaseAPIResponse> Register(RegisterRequest request)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var result = await _logic.AuthBusinessLogic.UserRegister(request);
            response.Data = result;
            return response;
        }
        catch (BusinessLogicException err)
        {
            response.RespCode = err.ResponseCode;
            response.RespDescription = err.Message;
            return response;
        }
        catch (Exception ex)
        {
            response.RespCode = "500";
            response.RespDescription = !string.IsNullOrEmpty(ex.InnerException?.Message) ? ex.InnerException.Message : ex.Message;
            return response;
        }
        finally
        {
            LoggingDTO log = new LoggingDTO()
            {
                ControllerName = nameof(AuthController),
                ActionName = nameof(Login),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [AllowAnonymous]
    [HttpPost("logout")]
    public async Task<BaseAPIResponse> Logout(LogoutDto request)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            await _services.TokenService.RevokeTokenAsync(request.RefreshToken);

            return response;
        }
        catch (BusinessLogicException err)
        {
            response.RespCode = err.ResponseCode;
            response.RespDescription = err.Message;
            return response;
        }
        catch (Exception ex)
        {
            response.RespCode = "500";
            response.RespDescription = !string.IsNullOrEmpty(ex.InnerException?.Message) ? ex.InnerException.Message : ex.Message;
            return response;
        }
        finally
        {
            LoggingDTO log = new LoggingDTO()
            {
                ControllerName = nameof(AuthController),
                ActionName = nameof(Logout),
                Message = response.RespDescription,
                //RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<BaseAPIResponse> Login(LoginDto request)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var user = await _unitOfWork.User.Get(x => x.UserName == request.UserName, "Account");

            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                throw new BusinessLogicException("401", "Invalid Credential.");
            }


            if (await _userManager.GetTwoFactorEnabledAsync(user))
            {
                var twoFactorToken = await _userManager.GenerateTwoFactorTokenAsync(user, "Authenticator");
                response.RespCode = "012";
                response.RespDescription = "Require two factor authentication.";
                return response;
            }

            var result = await _logic.AuthBusinessLogic.Login(user);
            response.Data = result;
            return response;
        }
        catch (BusinessLogicException err)
        {
            response.RespCode = err.ResponseCode;
            response.RespDescription = err.Message;
            return response;
        }
        catch (Exception ex)
        {
            response.RespCode = "500";
            response.RespDescription = ex.Message;
            return response;
        }
        finally
        {
            LoggingDTO log = new LoggingDTO()
            {
                ControllerName = nameof(AuthController),
                ActionName = nameof(Login),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }

    }

    [AllowAnonymous]
    [HttpPost("2fa-login")]
    public async Task<BaseAPIResponse> TwoFaLogin(TwoFaLoginDto request)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var user = await _unitOfWork.User.Get(x => x.UserName == request.UserName, "Account");

            if (user == null)
            {
                throw new BusinessLogicException("401", "Invalid email or token");
            }
            var isValid2FAToken = await _userManager.VerifyTwoFactorTokenAsync(user, "Authenticator", request.TwoFactorCode);
            if (!isValid2FAToken)
            {
                throw new BusinessLogicException("401", "Invalid 2FA code.");
            }
            var result = await _logic.AuthBusinessLogic.Login(user);
            response.Data = result;
            return response;
        }
        catch (BusinessLogicException err)
        {
            response.RespCode = err.ResponseCode;
            response.RespDescription = err.Message;
            return response;
        }
        catch (Exception ex)
        {
            response.RespCode = "500";
            response.RespDescription = ex.Message;
            return response;
        }
        finally
        {
            LoggingDTO log = new LoggingDTO()
            {
                ControllerName = nameof(AuthController),
                ActionName = nameof(Login),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [HttpGet("2fa-auth")]
    [Authorize]
    public async Task<BaseAPIResponse> Get2FaAuthentication()
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
                if (user == null)
                {
                    response.RespCode = "404";
                    response.RespDescription = "Invalid User!";
                    return response;
                }
            }

            var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            }
            var qrCodeUri = Helper.GenerateQrCodeUri(user.Email, unformattedKey);

            var result = new TwoFaResponse
            {
                QrCode = GenerateQrCode(qrCodeUri),
            };

            response.Data = result;
            return response;
        }
        catch (BusinessLogicException err)
        {
            response.RespCode = err.ResponseCode;
            response.RespDescription = err.Message;
            return response;
        }
        catch (Exception ex)
        {
            response.RespCode = "500";
            response.RespDescription = ex.Message;
            return response;
        }
        finally
        {
            LoggingDTO log = new LoggingDTO()
            {
                ControllerName = nameof(AuthController),
                ActionName = nameof(Get2FaAuthentication),
                Message = response.RespDescription,
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }

    }

    [HttpPost("enable-2fa-auth")]
    [Authorize]
    public async Task<BaseAPIResponse> Enabel2FaAuth(TwoFaRequest request)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            bool isRest2FA = false;
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
                isRest2FA = true;
                if (user == null)
                {
                    response.RespCode = "404";
                    response.RespDescription = "Invalid User!";
                    return response;
                }
            }

            var verificationCode = request.VerificationCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
                user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

            if (!is2faTokenValid)
            {
                ModelState.AddModelError("model.VerificationCode", "Verification code is invalid.");
                response.RespCode = "400";
                response.RespDescription = "Verification code is invalid.";
                return response;
            }

            await _userManager.SetTwoFactorEnabledAsync(user, true);

            if (isRest2FA)
            {
                await _signInManager.SignOutAsync();
            }
            return response;
        }
        catch (BusinessLogicException err)
        {
            response.RespCode = err.ResponseCode;
            response.RespDescription = err.Message;
            return response;
        }
        catch (Exception ex)
        {
            response.RespCode = "500";
            response.RespDescription = ex.Message;
            return response;
        }
        finally
        {
            LoggingDTO log = new LoggingDTO()
            {
                ControllerName = nameof(AuthController),
                ActionName = nameof(Enabel2FaAuth),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [HttpPost("disable-2fa-auth")]
    [Authorize]
    public async Task<BaseAPIResponse> Disable2FaAuth()
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                response.RespCode = "404";
                response.RespDescription = "Invalid User!";
                return response;
            }

            await _userManager.SetTwoFactorEnabledAsync(user, false);
            await _userManager.ResetAuthenticatorKeyAsync(user);
            return response;
        }
        catch (BusinessLogicException err)
        {
            response.RespCode = err.ResponseCode;
            response.RespDescription = err.Message;
            return response;
        }
        catch (Exception ex)
        {
            response.RespCode = "500";
            response.RespDescription = ex.Message;
            return response;
        }
        finally
        {
            LoggingDTO log = new LoggingDTO()
            {
                ControllerName = nameof(AuthController),
                ActionName = nameof(Enabel2FaAuth),
                Message = response.RespDescription,
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }


    }

    [AllowAnonymous]
    [HttpPost("refreshtoken")]
    public async Task<BaseAPIResponse> RefreshToken(RefreshTokenDto request)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var result = await _services.TokenService.RefreshTokenAsync(request.Token, request.RefreshToken);
            response.Data = result;
            return response;
        }
        catch (BusinessLogicException err)
        {
            response.RespCode = err.ResponseCode;
            response.RespDescription = err.Message;
            return response;
        }
        catch (Exception ex)
        {
            response.RespCode = "500";
            response.RespDescription = ex.Message;
            return response;
        }
        finally
        {
            LoggingDTO log = new LoggingDTO()
            {
                ControllerName = nameof(AuthController),
                ActionName = nameof(RefreshToken),
                Message = response.RespDescription,
                RequestData = JsonSerializer.Serialize(request),
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [Authorize]
    [HttpGet("get-users")]
    public async Task<BaseAPIResponse> GetUsers()
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var claims = User.Claims.Where(x => x.Type == "AccountId").FirstOrDefault()?.Value;
            if (string.IsNullOrEmpty(claims))
            {
                response.RespCode = "404";
                response.RespDescription = "Invalid User! User is not Found";
                return response;
            }

            int accountId = int.Parse(claims);

            var result = await _logic.AuthBusinessLogic.GetUsers(accountId);
            response.Data = result;
            return response;
        }
        catch (BusinessLogicException err)
        {
            response.RespCode = err.ResponseCode;
            response.RespDescription = err.Message;
            return response;
        }
        catch (Exception ex)
        {
            response.RespCode = "500";
            response.RespDescription = ex.Message;
            return response;
        }
        finally
        {
            LoggingDTO log = new LoggingDTO()
            {
                ControllerName = nameof(AuthController),
                ActionName = nameof(GetUsers),
                Message = response.RespDescription,
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [Authorize]
    [HttpPost("register-recruiter")]
    public async Task<BaseAPIResponse> RegisterRecruiter(RegisterRecruiterDto request)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var claims = User.Claims.Where(x => x.Type == "AccountId").FirstOrDefault()?.Value;
            if (string.IsNullOrEmpty(claims))
            {
                response.RespCode = "404";
                response.RespDescription = "Invalid User! User is not Found";
                return response;
            }

            int accountId = int.Parse(claims);

            var result = await _logic.AuthBusinessLogic.RegisterRecruiter(request, accountId);
            response.Data = result;
            return response;
        }
        catch (BusinessLogicException err)
        {
            response.RespCode = err.ResponseCode;
            response.RespDescription = err.Message;
            return response;
        }
        catch (Exception ex)
        {
            response.RespCode = "500";
            response.RespDescription = ex.Message;
            return response;
        }
        finally
        {
            LoggingDTO log = new LoggingDTO()
            {
                ControllerName = nameof(AuthController),
                ActionName = nameof(GetUsers),
                Message = response.RespDescription,
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    [Authorize]
    [HttpPost("edit-recruiter")]
    public async Task<BaseAPIResponse> EditRecruiter(EidtRecruiterDto request)
    {
        BaseAPIResponse response = CreateResponse(null);
        try
        {
            var claims = User.Claims.Where(x => x.Type == "AccountId").FirstOrDefault()?.Value;
            if (string.IsNullOrEmpty(claims))
            {
                response.RespCode = "404";
                response.RespDescription = "Invalid User! User is not Found";
                return response;
            }

            int accountId = int.Parse(claims);

            var result = await _logic.AuthBusinessLogic.EditRecruiter(request, accountId);
            response.Data = result;
            return response;
        }
        catch (BusinessLogicException err)
        {
            response.RespCode = err.ResponseCode;
            response.RespDescription = err.Message;
            return response;
        }
        catch (Exception ex)
        {
            response.RespCode = "500";
            response.RespDescription = ex.Message;
            return response;
        }
        finally
        {
            LoggingDTO log = new LoggingDTO()
            {
                ControllerName = nameof(AuthController),
                ActionName = nameof(GetUsers),
                Message = response.RespDescription,
                ResponseData = JsonSerializer.Serialize(response)
            };
            await _unitOfWork.Logging(log);
        }
    }

    private string GenerateQrCode(string qrCodeUri)
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(qrCodeUri, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new QRCode(qrCodeData);
        using var qrCodeImage = qrCode.GetGraphic(20);
        using var ms = new MemoryStream();
        qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        return $"data:image/png;base64,{Convert.ToBase64String(ms.ToArray())}";
    }
}

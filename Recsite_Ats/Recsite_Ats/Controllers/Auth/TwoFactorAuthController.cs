using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using Recsite_Ats.Domain.Entites;
using Recsite_Ats.Domain.ViewModels;
using System.Text.Encodings.Web;

namespace Recsite_Ats.Web.Controllers.Auth;
public class TwoFactorAuthController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    SignInManager<ApplicationUser> _signInManager;
    private readonly UrlEncoder _urlEncoder;

    private const string AuthenticatorUriFormat =
        "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

    public TwoFactorAuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, UrlEncoder urlEncoder)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _urlEncoder = urlEncoder;
    }

    [HttpGet]
    public async Task<IActionResult> EnableAuthenticator()
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
                if (user == null)
                {
                    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                }
            }

            var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            }

            var model = new EnableAuthenticatorViewModel
            {
                AuthenticatorUri = GenerateQrCodeUri(user.Email, unformattedKey)
            };

            ViewBag.QrCodeImage = GenerateQrCode(model.AuthenticatorUri);

            return View(model);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
        finally
        {

        }

    }

    [HttpPost]
    public async Task<IActionResult> EnableAuthenticator(EnableAuthenticatorViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        bool isRest2FA = false;
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            isRest2FA = true;
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
        }

        var verificationCode = model.VerificationCode.Replace(" ", string.Empty).Replace("-", string.Empty);

        var is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
            user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

        if (!is2faTokenValid)
        {
            ModelState.AddModelError("model.VerificationCode", "Verification code is invalid.");
            return View(model);
        }

        await _userManager.SetTwoFactorEnabledAsync(user, true);

        if (isRest2FA)
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        return RedirectToAction(nameof(ManageTwoFactor));
    }

    [HttpGet]
    public async Task<IActionResult> DisableAuthenticator()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        await _userManager.SetTwoFactorEnabledAsync(user, false);
        await _userManager.ResetAuthenticatorKeyAsync(user);

        return RedirectToAction(nameof(ManageTwoFactor));
    }

    [HttpGet]
    public async Task<IActionResult> ManageTwoFactor()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var model = new ManageTwoFactorViewModel
        {
            HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null,
            Is2faEnabled = await _userManager.GetTwoFactorEnabledAsync(user),
            RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user)
        };

        return View(model);
    }

    private string GenerateQrCodeUri(string email, string unformattedKey)
    {
        return string.Format(
            AuthenticatorUriFormat,
            _urlEncoder.Encode("GoogleAuthenticator"),
            _urlEncoder.Encode(email),
            unformattedKey);
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

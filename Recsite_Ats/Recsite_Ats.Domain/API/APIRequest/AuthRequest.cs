using System.ComponentModel.DataAnnotations;

namespace Recsite_Ats.Domain.API.APIRequest;
public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; } = false;
}

public class RegisterRequest
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string CompanyName { get; set; }
    public string Password { get; set; }
    public string? Phone { get; set; }
    public string? Role { get; set; }
}

public class RefreshTokenRequest
{
    [Required]
    public string Token { get; set; }
    [Required]
    public string RefreshToken { get; set; }
}

public class RevokerTokenRequest
{
    [Required]
    public string RevokerToken { get; set; }
}

public class TwoFaRequest
{
    [Required]
    [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Text)]
    [Display(Name = "Verification Code")]
    public string VerificationCode { get; set; }
}

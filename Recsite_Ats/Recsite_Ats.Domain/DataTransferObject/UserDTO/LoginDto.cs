using System.ComponentModel.DataAnnotations;

namespace Recsite_Ats.Domain.DataTransferObject.UserDTO;
public class LoginDto
{
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
}

public class TwoFaLoginDto
{
    [Required]
    public string UserName { get; set; }
    [Required]
    public string TwoFactorCode { get; set; }
}
public class LogoutDto
{
    [Required]
    public string RefreshToken { get; set; }
}

public class RefreshTokenDto
{
    [Required]
    public string Token { get; set; }
    [Required]
    public string RefreshToken { get; set; }
}

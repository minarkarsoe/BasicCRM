namespace Recsite_Ats.Domain.API.APIResponse;
public class LoginResponse
{
    public string Username { get; set; }
    public string Token { get; set; }
}

public class RegisterResponse
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string CompanyName { get; set; }
}

public class TwoFaResponse
{
    public string QrCode { get; set; }
}

namespace Recsite_Ats.Domain.DataTransferObject.UserDTO;
public class RefreshTokenResponseDto
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public object UserData { get; set; } = null;
}

public class UserResponseDto
{
    public int AccountId { get; set; }
    public int UserId { get; set; }
    public int RoleId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string RoleName { get; set; }
    public DateTime CreatedDate { get; set; }
}

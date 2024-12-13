using System.ComponentModel.DataAnnotations;

namespace Recsite_Ats.Domain.DataTransferObject.UserDTO;
public class RegisterRecruiterDto
{
    [Required]
    public int AccountId { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    public string Phone { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }
}

public class EidtRecruiterDto
{
    [Required]
    public int UserId { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    public string Phone { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }
}

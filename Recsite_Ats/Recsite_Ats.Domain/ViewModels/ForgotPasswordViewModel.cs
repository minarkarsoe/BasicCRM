using System.ComponentModel.DataAnnotations;

namespace Recsite_Ats.Domain.ViewModels;
public class ForgotPasswordViewModel
{
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }
}

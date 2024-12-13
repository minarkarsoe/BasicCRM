using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Recsite_Ats.Domain.ViewModels;

public class EnableAuthenticatorViewModel
{
    public string AuthenticatorUri { get; set; }

    [Required]
    [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Text)]
    [Display(Name = "Verification Code")]
    public string VerificationCode { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace Recsite_Ats.Domain.ViewModels;

public class LoginWith2faViewModel
{
    [Required]
    [DataType(DataType.Text)]
    [Display(Name = "Authenticator code")]
    public string TwoFactorCode { get; set; }

    [Display(Name = "Remember this machine")]
    public bool RememberMachine { get; set; }

    public bool RememberMe { get; set; }
}

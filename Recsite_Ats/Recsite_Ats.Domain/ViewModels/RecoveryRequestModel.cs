using System.ComponentModel.DataAnnotations;

namespace Recsite_Ats.Domain.ViewModels;
public class RecoveryRequestModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string RecoveryType { get; set; }
}

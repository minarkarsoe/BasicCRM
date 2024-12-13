using System.ComponentModel.DataAnnotations;

namespace Recsite_Ats.Domain.ViewModels;
public class Reset2FAViewModel
{
    [Required]
    public string Code { get; set; }
}

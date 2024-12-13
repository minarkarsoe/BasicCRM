using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Recsite_Ats.Domain.ViewModels
{
    public class RegisterVM
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /* [Required]
         [DataType(DataType.Password)]
         [Compare(nameof(Password))]
         [Display(Name = "Confirm Password")]
         public string ConfirmPassword { get; set; }*/

        [Display(Name = "Phone Number")]
        public string? Phone { get; set; }

        public string? RedirectUrl { get; set; }

        public string Role { get; set; }

        public string Subscription { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem>? RoleList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem?> SubscriptionList { get; set; }

        [ValidateNever]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

    }
}

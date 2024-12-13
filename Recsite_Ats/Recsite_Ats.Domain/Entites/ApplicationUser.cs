using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Recsite_Ats.Domain.Entites;
public class ApplicationUser : IdentityUser<int>
{
    public int? AccountId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public int CreatedBy { get; set; }
    public DateTime? EditedDate { get; set; }
    public int? EditedBy { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    [ForeignKey(nameof(AccountId))]
    [JsonIgnore]
    public Account Account { get; set; }
    public ICollection<UserToken> UserTokens { get; set; }
    public ICollection<CompanyFollowers> CompanyFollowers { get; set; }
}

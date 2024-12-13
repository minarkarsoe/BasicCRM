using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recsite_Ats.Domain.Entites;
public class UserToken
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string JwtToken { get; set; }
    [Required]
    public string RefreshToken { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool IsRevoked { get; set; } = false;
    [Required]
    public int UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public ApplicationUser User { get; set; }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recsite_Ats.Domain.Entites;
public class CompanyFollowers
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CompanyId { get; set; }
    [ForeignKey(nameof(CompanyId))]
    public Company Company { get; set; }
    [ForeignKey(nameof(UserId))]
    public ApplicationUser User { get; set; }
}

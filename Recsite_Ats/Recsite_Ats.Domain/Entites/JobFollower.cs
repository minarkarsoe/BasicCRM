using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recsite_Ats.Domain.Entites;
public class JobFollower
{
    [Key]
    public int Id { get; set; }
    [DefaultValue(0)]
    public int JobId { get; set; }
    [DefaultValue(0)]
    public int UserId { get; set; }

    [NotMapped]
    [ForeignKey(nameof(JobId))]
    public Job? Jobs { get; set; }
    [NotMapped]
    [ForeignKey(nameof(UserId))]
    public ApplicationUser User { get; set; }
}


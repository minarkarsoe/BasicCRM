using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recsite_Ats.Domain.Entites;
public class JobApplication
{
    [Key]
    public int Id { get; set; }
    [DefaultValue(0)]
    public int JobId { get; set; }
    [DefaultValue(0)]
    public int CandidateId { get; set; }

    [NotMapped]
    [ForeignKey(nameof(JobId))]
    public virtual Job Jobs { get; set; }
    [NotMapped]
    [ForeignKey(nameof(CandidateId))]
    public virtual Candidate Candidate { get; set; }
}
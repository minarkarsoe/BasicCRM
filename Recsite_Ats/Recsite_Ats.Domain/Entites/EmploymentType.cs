using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recsite_Ats.Domain.Entites;
public class EmploymentType
{
    [Key]
    public int Id { get; set; }
    public int? AccountId { get; set; }
    public string Title { get; set; }
    [NotMapped]
    public virtual Account Account { get; set; }

    public virtual ICollection<Job> Jobs { get; set; }
}


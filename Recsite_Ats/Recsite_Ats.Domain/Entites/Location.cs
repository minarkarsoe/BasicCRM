using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recsite_Ats.Domain.Entites;
public class Location
{
    [Key]
    public int Id { get; set; }

    [DefaultValue(0)]
    public int AccountId { get; set; }
    [DefaultValue(0)]
    public int ParentId { get; set; }
    public string Title { get; set; }
    [NotMapped]
    [ForeignKey(nameof(AccountId))]
    public virtual Account Account { get; set; }
    public virtual ICollection<Job> Jobs { get; set; }
}


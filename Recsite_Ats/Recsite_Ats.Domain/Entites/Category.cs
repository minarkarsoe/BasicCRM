using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recsite_Ats.Domain.Entites;
public class Category
{
    [Key]
    public int Id { get; set; }
    public int? AccountId { get; set; }
    public int? ParentCategoryId { get; set; }
    public string Title { get; set; }

    [NotMapped]
    public virtual Account Account { get; set; }

    public virtual ICollection<Job> Jobs { get; set; }
    public ICollection<Skill> Skills { get; set; }

}


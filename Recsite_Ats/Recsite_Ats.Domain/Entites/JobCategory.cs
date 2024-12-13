using System.ComponentModel.DataAnnotations;

namespace Recsite_Ats.Domain.Entites;
public class JobCategory
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }

    public ICollection<JobSubCategory> JobSubCategories { get; set; }

}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Recsite_Ats.Domain.Entites;
public class JobSubCategory
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public int JobCategoryId { get; set; }
    [ForeignKey(nameof(JobCategoryId))]
    [JsonIgnore]
    public JobCategory JobCategory { get; set; }
}

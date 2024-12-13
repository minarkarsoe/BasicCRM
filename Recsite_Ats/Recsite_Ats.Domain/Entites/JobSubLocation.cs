using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Recsite_Ats.Domain.Entites;
public class JobSubLocation
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public int JobLocationId { get; set; }
    [ForeignKey(nameof(JobLocationId))]
    [JsonIgnore]
    public JobLocation JobLocation { get; set; }
}

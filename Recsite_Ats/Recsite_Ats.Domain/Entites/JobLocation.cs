using System.ComponentModel.DataAnnotations;

namespace Recsite_Ats.Domain.Entites;
public class JobLocation
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public ICollection<JobSubLocation> JobSubLocations { get; set; }

}

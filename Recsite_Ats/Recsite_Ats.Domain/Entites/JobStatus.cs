using System.ComponentModel.DataAnnotations;

namespace Recsite_Ats.Domain.Entites;
public class JobStatus
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public int Sort { get; set; }
    public bool IsCustomized { get; set; }
}

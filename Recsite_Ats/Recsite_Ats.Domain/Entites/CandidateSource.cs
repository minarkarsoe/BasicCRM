using System.ComponentModel.DataAnnotations;

namespace Recsite_Ats.Domain.Entites;
public class CandidateSource
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsCustomized { get; set; }

}

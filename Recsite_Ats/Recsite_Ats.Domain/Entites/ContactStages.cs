using System.ComponentModel.DataAnnotations;

namespace Recsite_Ats.Domain.Entites;
public class ContactStages
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsCustomized { get; set; }
    public bool IsDefault { get; set; }
    public int Sort { get; set; }
}

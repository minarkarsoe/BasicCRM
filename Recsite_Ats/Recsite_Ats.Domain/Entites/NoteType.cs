using System.ComponentModel.DataAnnotations;

namespace Recsite_Ats.Domain.Entites;
public class NoteType
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsDefault { get; set; } = false;
    public bool IsCustomize { get; set; }
}

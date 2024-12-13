using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Recsite_Ats.Domain.Entites;
public class CompanyNotes
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int CompanyId { get; set; }
    [Required]
    public int NoteId { get; set; }

    [JsonIgnore]
    public Company Company { get; set; }
    [JsonIgnore]
    public Note Note { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace Recsite_Ats.Domain.Entites;
public class Note
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int EditedBy { get; set; }
    public DateTime EditedDate { get; set; }
    public ICollection<CompanyNotes> CompanyNotes { get; set; }
}

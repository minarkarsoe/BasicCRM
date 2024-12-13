using System.ComponentModel.DataAnnotations;

namespace Recsite_Ats.Domain.Entites;
public class Document
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }
    [Required]
    public string FilePath { get; set; }
    [Required]
    public string Type { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int EditedBy { get; set; }
    public DateTime EditedDate { get; set; }

    public ICollection<CompanyDocuments> CompanyDocuments { get; set; }
}

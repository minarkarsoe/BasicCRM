using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Recsite_Ats.Domain.Entites;
public class CompanyContacts
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int CompanyId { get; set; }
    [JsonIgnore]
    public Company Company { get; set; }
    [Required]
    public int ContactId { get; set; }
    [JsonIgnore]
    public Contact Contact { get; set; }

    public bool IsPrimary { get; set; } = false;
}


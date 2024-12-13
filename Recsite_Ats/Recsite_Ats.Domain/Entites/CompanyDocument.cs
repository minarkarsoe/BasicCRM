using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Recsite_Ats.Domain.Entites;
public class CompanyDocuments
{
    [Key]
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public int DocumentId { get; set; }
    [JsonIgnore]
    public Company Company { get; set; }
    [JsonIgnore]
    public Document Document { get; set; }
}

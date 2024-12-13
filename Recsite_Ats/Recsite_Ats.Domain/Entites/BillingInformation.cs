using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Recsite_Ats.Domain.Entites;
public class BillingInformation
{
    [Key]
    public int Id { get; set; }
    public int AccountId { get; set; }
    public string CompanyName { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string ZipCode { get; set; }
    public string? VATNumber { get; set; }
    [ForeignKey(nameof(AccountId))]
    [JsonIgnore]
    public Account Account { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? UpdatedBy { get; set; }
}

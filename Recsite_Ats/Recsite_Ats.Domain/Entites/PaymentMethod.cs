using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Recsite_Ats.Domain.Entites;
public class PaymentMethod
{
    [Key]
    public int Id { get; set; }
    public int AccountId { get; set; }
    public string Name { get; set; }
    public string CardNumber { get; set; }
    public string SecurityCode { get; set; }
    public string ExpiryMonth { get; set; }
    public string ExpiryYear { get; set; }
    [ForeignKey(nameof(AccountId))]
    [JsonIgnore]
    public Account Account { get; set; }
}

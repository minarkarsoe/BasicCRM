using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recsite_Ats.Domain.Entites;
public class Subscription
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int DurationInDays { get; set; }
    [DefaultValue(0)]
    public int Tier { get; set; }
    [NotMapped]
    public List<UserSubscription> UserSubscriptions { get; set; }

}

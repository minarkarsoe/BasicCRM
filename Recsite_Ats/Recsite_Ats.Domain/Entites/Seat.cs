using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recsite_Ats.Domain.Entites;
public class Seat
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int AccountId { get; set; }
    [ForeignKey(nameof(AccountId))]
    public virtual Account Account { get; set; }
    [Required]
    public int TotalSeats { get; set; }
    [Required]
    public int UsedSeats { get; set; }
    [Required]
    public DateTime SeatRenewelDate { get; set; }
    [Required]
    public decimal SeatRenewelAmount { get; set; }
}

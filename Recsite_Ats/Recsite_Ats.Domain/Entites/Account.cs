using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recsite_Ats.Domain.Entites;
public class Account
{
    [Key]
    public int Id { get; set; }
    public string? PrimaryCountry { get; set; }
    public string? CompanyName { get; set; }
    public string? ContactFirstName { get; set; }
    public string? ContactLastName { get; set; }
    public string? ContactPhone { get; set; }
    public string? ContactEmail { get; set; }
    public DateTime CreatedDate { get; set; }
    [DefaultValue(0)]
    public int CreatedBy { get; set; }
    public DateTime? EditedDate { get; set; }
    [DefaultValue(0)]
    public int EditedBy { get; set; }
    public int SeatId { get; set; }
    [ForeignKey(nameof(SeatId))]
    public virtual Seat Seat { get; set; }
    public ICollection<Company> AccountCompanies { get; set; }
    public ICollection<Contact> AccountContacts { get; set; }
    public ICollection<Skill> Skills { get; set; }
    public ICollection<Category> Categories { get; set; }
    public ICollection<ApplicationUser> ApplicationUsers { get; set; }
}


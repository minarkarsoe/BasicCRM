using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recsite_Ats.Domain.Entites;
[Table("Companies")]
public class Company
{
    [Key]
    public int Id { get; set; }
    [DefaultValue(0)]
    public int AccountId { get; set; }
    public string? PrimaryContact { get; set; }
    public string CompanyName { get; set; }
    public string LegalName { get; set; }
    public string Logo { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? TwitterUrl { get; set; }
    public string? FacebookUrl { get; set; }
    public string? Website { get; set; }
    public DateTime CreatedDate { get; set; }
    [DefaultValue(0)]
    public int CreatedBy { get; set; }
    public DateTime? EditedDate { get; set; }
    [DefaultValue(0)]
    public int EditedBy { get; set; }

    [NotMapped]
    [ForeignKey(nameof(AccountId))]
    public virtual Account Account { get; set; }
    public ICollection<Address> Addresses { get; set; }
    public ICollection<CompanyContacts> CompanyContacts { get; set; }
    public ICollection<CompanyNotes> CompanyNotes { get; set; }
    public ICollection<CompanyDocuments> CompanyDocuments { get; set; }
    public ICollection<CompanyFollowers> CompanyFollowers { get; set; }
}


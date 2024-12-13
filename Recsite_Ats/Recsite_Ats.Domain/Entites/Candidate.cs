using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recsite_Ats.Domain.Entites;
public class Candidate
{
    [Key]
    public int Id { get; set; }
    [DefaultValue(0)]
    public int AccountId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
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
    public ICollection<JobApplication> Applications { get; set; }
}


using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recsite_Ats.Domain.Entites;
public class Address
{
    [Key]
    public int Id { get; set; }
    [DefaultValue(0)]
    public int CompanyId { get; set; }
    [DefaultValue(0)]
    public int ContactId { get; set; }
    [DefaultValue(0)]
    public int CandidateId { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? ZipCode { get; set; }
    public string? AddressType { get; set; }
    public DateTime CreatedDate { get; set; }
    [DefaultValue(0)]
    public int CreatedBy { get; set; }
    public DateTime? EditedDate { get; set; }
    public int? EditedBy { get; set; }

    [NotMapped]
    [ForeignKey(nameof(CompanyId))]
    public virtual Company Company { get; set; }
    [NotMapped]
    [ForeignKey(nameof(ContactId))]
    public virtual Contact Contact { get; set; }
    [NotMapped]
    [ForeignKey(nameof(CandidateId))]
    public virtual Candidate Candidate { get; set; }
}


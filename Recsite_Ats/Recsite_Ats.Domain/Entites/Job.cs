using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recsite_Ats.Domain.Entites;
public class Job
{
    [Key]
    public int Id { get; set; }
    public int AccountId { get; set; }
    public int JobOwnerUserId { get; set; }
    public string? Title { get; set; }
    public string? Summary { get; set; }
    public int CategoryId { get; set; }
    public int LocationId { get; set; }
    public int EmploymentTypeId { get; set; }
    public string? JobBenefits { get; set; }
    public string? WorkingHour { get; set; }
    public string? Skills { get; set; }
    public string? Responsibilities { get; set; }
    public string? Qualifications { get; set; }
    public string? IncentiveCompensation { get; set; }
    public string? ExperienceRequirements { get; set; }
    public string? SalaryCurrency { get; set; }
    public string? SalaryUnit { get; set; }
    public decimal? SalaryMin { get; set; }
    public decimal? SalaryMax { get; set; }
    public string? PublicSalary { get; set; }
    public string? PublicDescription { get; set; }
    [DefaultValue(false)]
    public bool IsPublic { get; set; }
    public string? Benefits { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? StreetAddress { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostCode { get; set; }
    public string? CountryCode { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }

    public DateTime CreatedDate { get; set; }
    [DefaultValue(0)]
    public int CreatedBy { get; set; }
    public DateTime? EditedDate { get; set; }
    [DefaultValue(0)]
    public int EditedBy { get; set; }

    //public int JobStatusId { get; set; }  // Added for the relationship


    [ForeignKey(nameof(AccountId))]
    public virtual Account Account { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public virtual Category Category { get; set; }

    [ForeignKey(nameof(EmploymentTypeId))]
    public virtual EmploymentType EmploymentType { get; set; }

    [ForeignKey(nameof(LocationId))]
    public virtual Location Location { get; set; }
    /*  [NotMapped]
      public JobStatus? JobStatus { get; set; } // Navigation property for JobStatus
      */

    public ICollection<JobFollower>? JobFollowers { get; set; }
    public ICollection<JobApplication>? Applications { get; set; }
}


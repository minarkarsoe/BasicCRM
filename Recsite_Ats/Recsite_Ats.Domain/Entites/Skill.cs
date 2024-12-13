using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recsite_Ats.Domain.Entites;
public class Skill
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; }
    public int? PortalId { get; set; }
    public int? AccountId { get; set; }
    [DefaultValue(0)]
    public int CategoryId { get; set; }

    [NotMapped]
    [ForeignKey(nameof(CategoryId))]
    public Category Category { get; set; }
}


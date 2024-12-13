using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recsite_Ats.Domain.Entites;
public class FieldLayout
{
    [Key]
    public int Id { get; set; }
    public int? AccountId { get; set; }
    public string? TableName { get; set; }
    public int SectionLayoutId { get; set; }
    public string FieldName { get; set; }
    public Boolean Required { get; set; }
    public Boolean Visible { get; set; }
    public Boolean IsCustomField { get; set; } = false;
    public long Sort { get; set; }
    [NotMapped]
    public Account Account { get; set; }
    [NotMapped]
    [ForeignKey(nameof(SectionLayoutId))]
    public SectionLayout SectionLayout { get; set; }

}

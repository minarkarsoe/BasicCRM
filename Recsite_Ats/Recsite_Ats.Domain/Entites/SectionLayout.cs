using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recsite_Ats.Domain.Entites;
public class SectionLayout
{
    [Key]
    public int SectionLayoutId { get; set; }
    public string TableName { get; set; }
    public int? AccountId { get; set; }
    public string SectionName { get; set; }
    public bool Visible { get; set; } = true;
    public bool IsCustomSection { get; set; } = false;
    public int Sort { get; set; }
    [NotMapped]
    public Account Account { get; set; }
}

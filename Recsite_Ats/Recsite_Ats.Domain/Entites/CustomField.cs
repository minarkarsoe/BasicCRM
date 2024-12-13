using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recsite_Ats.Domain.Entites;
[Table("CustomField")]
public class CustomField
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int AccountId { get; set; }
    [Required]
    public string TableName { get; set; }
    [Required]
    public string FieldName { get; set; }
    [Required]
    public string FieldAlias { get; set; }
    [Required]
    public int FieldTypeId { get; set; }
    public string? ViewValues { get; set; }
    [NotMapped]
    [ForeignKey(nameof(AccountId))]
    public Account Account { get; set; }

    [ForeignKey(nameof(FieldTypeId))]
    public FieldType FieldType { get; set; }
}

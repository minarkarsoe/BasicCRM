using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recsite_Ats.Domain.Entites;
public class StoreCustomFieldValue
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int CustomFieldId { get; set; }
    [Required]
    public string TableName { get; set; }
    [Required]
    public int TableId { get; set; }
    [Required]
    public string StoreValue { get; set; }

    [ForeignKey(nameof(CustomFieldId))]
    public CustomField CustomField { get; set; }
}

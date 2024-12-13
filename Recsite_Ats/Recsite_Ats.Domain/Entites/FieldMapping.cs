using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recsite_Ats.Domain.Entites;
public class FieldMapping
{
    [Key]
    public int FieldMappingId { get; set; }
    public string TableName { get; set; }
    public string FieldName { get; set; }
    public int FieldTypeId { get; set; }
    [NotMapped]
    [ForeignKey(nameof(FieldTypeId))]
    public FieldType FieldType { get; set; }
}

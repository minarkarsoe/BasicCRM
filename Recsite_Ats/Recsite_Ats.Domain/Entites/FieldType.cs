using System.ComponentModel.DataAnnotations;

namespace Recsite_Ats.Domain.Entites;
public class FieldType
{
    [Key]
    public int Id { get; set; }
    public string FieldTypeName { get; set; }
}
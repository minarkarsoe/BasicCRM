namespace Recsite_Ats.Domain.Entites;
public class EmailTemplate
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public int ModuleId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Template { get; set; }
    public bool IsDefault { get; set; }
}

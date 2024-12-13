using Recsite_Ats.Domain.Entites;

namespace Recsite_Ats.Domain.DataTransferObject;
public class JobResponseDTO
{
    public IEnumerable<Job>? Jobs { get; set; }
    public SectionLayoutDTO? SectionLayout { get; set; }
}

public class JobRequestDTO
{
    public string TableName { get; set; }
    public int? AccountId { get; set; }
    public string Columns { get; set; }
    public int? JobId { get; set; }
}


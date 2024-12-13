using Recsite_Ats.Domain.Entites;

namespace Recsite_Ats.Domain.DataTransferObject;
public class CandidateResponseDTO
{
    public IEnumerable<Candidate>? Candidates { get; set; }
    public SectionLayoutDTO? SectionLayout { get; set; }
}

public class CandidateRequestDTO
{
    public string TableName { get; set; }
    public int? AccountId { get; set; }
    public string Columns { get; set; }
    public int? CandidateId { get; set; }
}

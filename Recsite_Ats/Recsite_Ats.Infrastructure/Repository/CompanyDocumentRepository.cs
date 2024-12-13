using Recsite_Ats.Application.Common.Interface.Repository;
using Recsite_Ats.Domain.Entites;
using Recsite_Ats.Infrastructure.Data;

namespace Recsite_Ats.Infrastructure.Repository;
public class CompanyDocumentRepository : Repository<CompanyDocuments>, ICompanyDocumentRepository
{
    private readonly ApplicationDbContext _db;
    public CompanyDocumentRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }
}

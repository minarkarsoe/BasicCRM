using Recsite_Ats.Application.Common.Interface.Repository;
using Recsite_Ats.Domain.Entites;
using Recsite_Ats.Infrastructure.Data;

namespace Recsite_Ats.Infrastructure.Repository;
public class DocumentRepository : Repository<Document>, IDocumentRepository
{
    private readonly ApplicationDbContext _db;
    public DocumentRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }
}

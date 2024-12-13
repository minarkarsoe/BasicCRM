using Recsite_Ats.Application.Common.Interface.Repository;
using Recsite_Ats.Domain.Entites;
using Recsite_Ats.Infrastructure.Data;

namespace Recsite_Ats.Infrastructure.Repository;
public class PaymentMethodRepository : Repository<PaymentMethod>, IPaymentMethodRepository
{
    private readonly ApplicationDbContext _db;
    public PaymentMethodRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }
}

using Recsite_Ats.Application.Common.Interface.Repository;
using Recsite_Ats.Domain.Entites;
using Recsite_Ats.Infrastructure.Data;

namespace Recsite_Ats.Infrastructure.Repository;
public class MailGunSettingRepository : Repository<MailGunSetting>, IMailGunSettingRepository
{
    private readonly ApplicationDbContext _context;
    public MailGunSettingRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
}

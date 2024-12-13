using Recsite_Ats.Application.Common.Interface.Businesslogic;
using Recsite_Ats.Application.Common.Interface.Repository;
using Recsite_Ats.Application.Common.Interface.Services;
using Recsite_Ats.Infrastructure.BusinessLogic;
using Recsite_Ats.Infrastructure.Data;
using Recsite_Ats.Infrastructure.Repository;
using Recsite_Ats.Infrastructure.Services;

namespace Recsite_Ats.Web.BuilderServices;

public static class RepositoryBuilderServices
{
    public static void BusinessLogicServices(this IServiceCollection services, IConfiguration configuration, ApplicationDbContext context)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IServices, Services>();
        services.AddScoped<IEmailSender, EmailSender>();
        services.AddScoped<IBusinessLogic, BusinessLogic>();

    }
}

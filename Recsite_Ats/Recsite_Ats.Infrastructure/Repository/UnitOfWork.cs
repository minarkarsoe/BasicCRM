using Microsoft.Extensions.Logging;
using Recsite_Ats.Application.Common.Interface.Repository;
using Recsite_Ats.Domain.DataTransferObject;
using Recsite_Ats.Infrastructure.Data;
using Serilog;

namespace Recsite_Ats.Infrastructure.Repository;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<UnitOfWork> _logger;
    public IApplicationUserRepository User { get; private set; }
    public ISubscriptionUserRepository UserSubscription { get; private set; }
    public ICompanyRepository Company { get; private set; }
    public ICandidateRepository Candidate { get; private set; }
    public IJobRepository Job { get; private set; }
    public IContactRepository Contact { get; private set; }
    public IAccountRepository Account { get; private set; }
    public ISectionLayoutRepository Section { get; private set; }
    public IFieldLayoutsRepository FieldLayout { get; private set; }
    public ICustomFieldRepository CustomField { get; private set; }
    public IFieldTypeRepository FieldType { get; private set; }
    public ICustomFieldStoreValue CustomFieldStoreValue { get; private set; }
    public ICompanyContactRepository CompanyContact { get; private set; }
    public INoteRepository Note { get; private set; }
    public ICompanyNoteRepository CompanyNote { get; private set; }
    public IDocumentRepository Document { get; private set; }
    public ICompanyDocumentRepository CompanyDocument { get; private set; }
    public IUserTokensRepository UserTokens { get; private set; }
    public ISeatRepository Seat { get; private set; }
    public ICompanyFollowersRepository Followers { get; private set; }
    public ICountryDataRepository CountryData { get; private set; }
    public IBillingInformationRepository BillingInformation { get; private set; }
    public IPaymentMethodRepository PaymentMethod { get; private set; }
    public INoteTypeRepository NoteType { get; private set; }
    public IJobStatusRepository JobStatus { get; private set; }
    public IDocumentTypeRepository DocumentType { get; private set; }
    public IContactStageRepository ContactStage { get; private set; }
    public ICandidateSourceRepository CandidateSource { get; private set; }
    public IJobCategoryRepository JobCategory { get; private set; }
    public IJobSubCategoryRepository JobSubCategory { get; private set; }
    public IJobLocationRepository JobLocation { get; private set; }
    public IJobSubLocationRepository JobSubLocation { get; private set; }
    public IApplicationModulesRepository ApplicationModules { get; private set; }
    public IEmailTemplateRepository EmailTemplate { get; private set; }
    public IMailGunSettingRepository MailGunSetting { get; private set; }
    public UnitOfWork(ApplicationDbContext db, ILogger<UnitOfWork> logger)
    {
        _db = db;
        _logger = logger;
        User = new ApplicationUserRepository(_db);
        UserSubscription = new SubscriptionUserRepository(_db);
        Company = new CompanyRepository(_db);
        Account = new AccountRepository(_db);
        Section = new SectionLayoutRepository(_db);
        FieldLayout = new FieldLayoutsRepository(_db);
        CustomField = new CustomFieldRepository(_db);
        FieldType = new FieldTypeRepository(_db);
        CustomFieldStoreValue = new CustomFieldStoreValueRepository(_db);
        Candidate = new CandidateRepository(_db);
        Job = new JobRepository(_db);
        Contact = new ContactRepository(_db);
        CompanyContact = new CompanyContactRepository(_db);
        Note = new NoteRepository(_db);
        CompanyNote = new CompanyNoteRepository(_db);
        Document = new DocumentRepository(_db);
        CompanyDocument = new CompanyDocumentRepository(_db);
        UserTokens = new UserTokensRepository(_db);
        Seat = new SeatRepository(_db);
        Followers = new CompanyFollowersRepository(_db);
        CountryData = new CountryDataRepository(_db);
        BillingInformation = new BillingInformationRepository(_db);
        PaymentMethod = new PaymentMethodRepository(_db);
        NoteType = new NoteTypeRepository(_db);
        JobStatus = new JobStatusRepository(_db);
        DocumentType = new DocumentTypeRepository(_db);
        ContactStage = new ContactStageRepository(_db);
        CandidateSource = new CandidateSourceRepository(_db);
        JobCategory = new JobCategroyRepository(_db);
        JobSubCategory = new JobSubCategoryRepository(_db);
        JobLocation = new JobLocationRepository(_db);
        JobSubLocation = new JobSubLocationRepository(_db);
        ApplicationModules = new ApplicationModulesRepository(_db);
        EmailTemplate = new EmailTemplateRepository(_db);
        MailGunSetting = new MailGunSettingRepository(_db);

    }

    public async Task Logging(LoggingDTO logger)
    {

        if (logger.Message != "Success")
        {
            Log.Error("Execite : Controller : {Controller} , Method : {Method} , Message : {message} , RequestData : {request} , ResponseData : {response}", logger.ControllerName, logger.ActionName, logger.Message, logger.RequestData, logger.ResponseData);
        }
        else
        {
            Log.Information("Execite : Controller : {Controller} , Method : {Method} , Message : {message} , RequestData : {request} , ResponseData : {response}", logger.ControllerName, logger.ActionName, logger.Message, logger.RequestData, logger.ResponseData);
        }
    }
    public async Task Save()
    {
        await _db.SaveChangesAsync();
    }
}

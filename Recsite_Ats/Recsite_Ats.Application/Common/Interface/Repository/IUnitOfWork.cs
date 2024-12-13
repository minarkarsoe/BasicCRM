using Recsite_Ats.Domain.DataTransferObject;

namespace Recsite_Ats.Application.Common.Interface.Repository;
public interface IUnitOfWork
{
    IApplicationUserRepository User { get; }
    ISubscriptionUserRepository UserSubscription { get; }
    ICompanyRepository Company { get; }
    ICandidateRepository Candidate { get; }
    IJobRepository Job { get; }
    IContactRepository Contact { get; }
    IAccountRepository Account { get; }
    ISectionLayoutRepository Section { get; }
    IFieldLayoutsRepository FieldLayout { get; }
    ICustomFieldRepository CustomField { get; }
    IFieldTypeRepository FieldType { get; }
    ICustomFieldStoreValue CustomFieldStoreValue { get; }
    ICompanyContactRepository CompanyContact { get; }
    INoteRepository Note { get; }
    ICompanyNoteRepository CompanyNote { get; }
    IDocumentRepository Document { get; }
    ICompanyDocumentRepository CompanyDocument { get; }
    IUserTokensRepository UserTokens { get; }
    ISeatRepository Seat { get; }
    ICompanyFollowersRepository Followers { get; }
    ICountryDataRepository CountryData { get; }
    IBillingInformationRepository BillingInformation { get; }
    IPaymentMethodRepository PaymentMethod { get; }
    INoteTypeRepository NoteType { get; }
    IJobStatusRepository JobStatus { get; }
    IDocumentTypeRepository DocumentType { get; }
    IContactStageRepository ContactStage { get; }
    ICandidateSourceRepository CandidateSource { get; }
    IJobCategoryRepository JobCategory { get; }
    IJobSubCategoryRepository JobSubCategory { get; }
    IJobLocationRepository JobLocation { get; }
    IJobSubLocationRepository JobSubLocation { get; }
    IApplicationModulesRepository ApplicationModules { get; }
    IEmailTemplateRepository EmailTemplate { get; }
    IMailGunSettingRepository MailGunSetting { get; }
    Task Logging(LoggingDTO log);
    Task Save();
}

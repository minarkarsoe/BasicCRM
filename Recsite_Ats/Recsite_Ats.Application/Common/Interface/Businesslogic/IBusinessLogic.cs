namespace Recsite_Ats.Application.Common.Interface.Businesslogic;
public interface IBusinessLogic
{
    IAdminSettingBusinessLogic AdminSettingBusinessLogic { get; }
    ICompanyBusinessLogic CompanyBusinessLogic { get; }
    ICandidateBusinesslogic CandidateBusinessLogic { get; }
    IJobBusinessLogic JobBusinessLogic { get; }
    IContactBusinessLogic ContactBusinessLogic { get; }
    IAuthBusinessLogic AuthBusinessLogic { get; }
    IDynamicFormBusinessLogic DynamicFormBusinessLogic { get; }
    IAdminTaxonomyBusinessLogic AdminTaxonomyBusinessLogic { get; }
}

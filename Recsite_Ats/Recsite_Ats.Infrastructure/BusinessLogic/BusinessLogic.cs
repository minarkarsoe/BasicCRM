using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Recsite_Ats.Application.Common.Interface.Businesslogic;
using Recsite_Ats.Application.Common.Interface.Repository;
using Recsite_Ats.Application.Common.Interface.Services;
using Recsite_Ats.Domain.Entites;

namespace Recsite_Ats.Infrastructure.BusinessLogic;
public class BusinessLogic : IBusinessLogic
{
    private readonly IServices _services;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IEmailSender _emailSender;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    public IAdminSettingBusinessLogic AdminSettingBusinessLogic { get; private set; }
    public ICompanyBusinessLogic CompanyBusinessLogic { get; private set; }
    public ICandidateBusinesslogic CandidateBusinessLogic { get; private set; }
    public IJobBusinessLogic JobBusinessLogic { get; private set; }
    public IContactBusinessLogic ContactBusinessLogic { get; private set; }
    public IAuthBusinessLogic AuthBusinessLogic { get; private set; }
    public IDynamicFormBusinessLogic DynamicFormBusinessLogic { get; private set; }
    public IAdminTaxonomyBusinessLogic AdminTaxonomyBusinessLogic { get; private set; }
    public BusinessLogic(
        IServices services,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<ApplicationRole> roleManager, IEmailSender emailSender)
    {
        _services = services;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _emailSender = emailSender;
        AdminSettingBusinessLogic = new AdminSettingBusinessLogic(_services, _unitOfWork, _mapper);
        CompanyBusinessLogic = new CompanyBusinessLogic(_unitOfWork, _services);
        AuthBusinessLogic = new AuthBusinessLogic(_unitOfWork, _services, _userManager, _roleManager, _signInManager);
        DynamicFormBusinessLogic = new DynamicFormBusinessLogic(_services, _unitOfWork);
        CandidateBusinessLogic = new CandidateBusinessLogic(_unitOfWork, _services);
        JobBusinessLogic = new JobBusinessLogic(_unitOfWork, _services);
        ContactBusinessLogic = new ContactBusinessLogic(_unitOfWork, _services, _emailSender);
        AdminTaxonomyBusinessLogic = new AdminTaxonomyBusinessLogic(_unitOfWork, _services);
    }

}

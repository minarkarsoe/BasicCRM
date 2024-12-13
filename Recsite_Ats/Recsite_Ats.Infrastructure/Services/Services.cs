using Amazon.S3;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Recsite_Ats.Application.Common.Interface.Repository;
using Recsite_Ats.Application.Common.Interface.Services;
using Recsite_Ats.Domain.DataTransferObject;
using Recsite_Ats.Domain.Entites;

namespace Recsite_Ats.Infrastructure.Services;
public class Services : IServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _config;
    private readonly IOptions<SmtpSettingsDTO> _smtpSettings;
    private readonly IOptions<MailGun> _mailgun;
    private readonly IAmazonS3 _s3service;
    private readonly UserManager<ApplicationUser> _userManager;
    public IUserService UserService { get; private set; }
    public ISettingService SettingService { get; private set; }
    public IAccountService AccountService { get; private set; }
    public IS3Service S3Service { get; private set; }
    public ITokenService TokenService { get; private set; }
    public IAdminService AdminService { get; private set; }

    public Services(IUnitOfWork unitOfWork, IConfiguration config, IOptions<SmtpSettingsDTO> smtpSettings, IOptions<MailGun> mailgun, IAmazonS3 amazonS3, UserManager<ApplicationUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _config = config;
        _smtpSettings = smtpSettings;
        _s3service = amazonS3;
        _mailgun = mailgun;
        _userManager = userManager;
        SettingService = new SettingService(_config, _unitOfWork);
        AccountService = new AccountService(_unitOfWork);
        S3Service = new S3Service(config);
        TokenService = new TokenService(_userManager, _config, _unitOfWork);
        AdminService = new AdminService(_userManager);

    }
}

namespace Recsite_Ats.Application.Common.Interface.Services;
public interface IServices
{
    IUserService UserService { get; }
    ISettingService SettingService { get; }
    IAccountService AccountService { get; }
    IS3Service S3Service { get; }
    ITokenService TokenService { get; }
    IAdminService AdminService { get; }
    //IBackgroundEmailService BackgroundEmailService { get; }
}

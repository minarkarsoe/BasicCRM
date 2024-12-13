using Recsite_Ats.Domain.API.APIRequest;
using Recsite_Ats.Domain.API.APIResponse;
using Recsite_Ats.Domain.DataTransferObject.UserDTO;
using Recsite_Ats.Domain.Entites;


namespace Recsite_Ats.Application.Common.Interface.Businesslogic;
public interface IAuthBusinessLogic
{
    Task<RegisterResponse> UserRegister(RegisterRequest request);
    Task<RefreshTokenResponseDto> Login(ApplicationUser user);
    Task<List<UserResponseDto>> GetUsers(int accountId);
    Task<RegisterResponse> RegisterRecruiter(RegisterRecruiterDto request, int accountId);
    Task<RegisterResponse> EditRecruiter(EidtRecruiterDto request, int accountId);
}

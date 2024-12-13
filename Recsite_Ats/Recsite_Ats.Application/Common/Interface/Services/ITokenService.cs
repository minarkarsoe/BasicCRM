using Recsite_Ats.Domain.DataTransferObject.UserDTO;
using Recsite_Ats.Domain.Entites;

namespace Recsite_Ats.Application.Common.Interface.Services;
public interface ITokenService
{
    Task<string> GenerateJwtToken(ApplicationUser user);
    Task<string> GenerateRefreshToken(ApplicationUser user, string token);
    Task<RefreshTokenResponseDto> RefreshTokenAsync(string token, string refreshToken);
    Task RevokeTokenAsync(string refreshToken);
}

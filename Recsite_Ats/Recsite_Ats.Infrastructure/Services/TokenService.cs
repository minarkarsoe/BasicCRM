using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Recsite_Ats.Application.Common.Interface.Repository;
using Recsite_Ats.Application.Common.Interface.Services;
using Recsite_Ats.Domain.DataTransferObject.UserDTO;
using Recsite_Ats.Domain.Entites;
using Recsite_Ats.Infrastructure.CustomException;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Recsite_Ats.Infrastructure.Services;
public class TokenService : ITokenService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;
    public TokenService(UserManager<ApplicationUser> userManager, IConfiguration configuration, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _configuration = configuration;
        _unitOfWork = unitOfWork;
    }
    public async Task<string> GenerateJwtToken(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("AccountId" , user.AccountId.ToString()),
            new Claim("UserId", user.Id.ToString()),
            new Claim("CompanyName" , user.Account.CompanyName)
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
           claims: claims,
           expires: DateTime.Now.AddMinutes(30),
           signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<string> GenerateRefreshToken(ApplicationUser user, string token)
    {
        var refreshToken = new UserToken
        {
            UserId = user.Id,
            JwtToken = token,
            RefreshToken = Guid.NewGuid().ToString(),
            ExpirationDate = DateTime.UtcNow.AddDays(7),
            IsRevoked = false
        };

        await _unitOfWork.UserTokens.Add(refreshToken);
        await _unitOfWork.Save();

        return refreshToken.RefreshToken;
    }

    public async Task<RefreshTokenResponseDto> RefreshTokenAsync(string token, string refreshToken)
    {
        var storedToken = await _unitOfWork.UserTokens.Get(t => t.RefreshToken == refreshToken && t.JwtToken == token);

        if (storedToken == null || storedToken.IsRevoked || storedToken.ExpirationDate <= DateTime.Now)
            throw new BusinessLogicException("400", "Invalid or expired refresh token.");

        storedToken.IsRevoked = true;
        await _unitOfWork.UserTokens.Update(storedToken);
        await _unitOfWork.Save();

        var user = await _userManager.FindByIdAsync(storedToken.UserId.ToString());
        var newJwtToken = await GenerateJwtToken(user);
        var newRefreshToken = await GenerateRefreshToken(user, newJwtToken);

        var result = new RefreshTokenResponseDto()
        {
            RefreshToken = newJwtToken,
            Token = newRefreshToken,
            UserData = new
            {
                TwoFactorRequired = true,
                FullName = user?.UserName,
                UserEmail = user?.Email,
                CompanyName = user?.Account.CompanyName
            }
        };

        return result;
    }

    public async Task RevokeTokenAsync(string refreshToken)
    {
        var storedToken = await _unitOfWork.UserTokens.Get(t => t.RefreshToken == refreshToken);

        if (storedToken == null || storedToken.IsRevoked)
            throw new BusinessLogicException("400", "Invalid or already revoked refresh token.");

        storedToken.IsRevoked = true;
        await _unitOfWork.UserTokens.Update(storedToken);
        await _unitOfWork.Save();
    }

}

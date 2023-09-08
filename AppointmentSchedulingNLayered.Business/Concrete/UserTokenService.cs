using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AppointmentSchedulingNLayered.Business.Abstract;
using AppointmentSchedulingNLayered.DataAccess.Abstract.EntityFramework;
using AppointmentSchedulingNLayered.Common.Jwt;
using AppointmentSchedulingNLayered.Entities.Concrete;

namespace AppointmentSchedulingNLayered.Business.Concrete;

public class UserTokenService : IUserTokenService {
    private IAppUserTokenRepository _appUserTokenRepository;
    private JwtSettings _jwtSettings;
    public UserTokenService(IAppUserTokenRepository appUserTokenRepository, IOptions<JwtSettings> jwtOptions) {
        _appUserTokenRepository = appUserTokenRepository;
        _jwtSettings = jwtOptions.Value;
    }

    public async Task AddAsync(AppUserToken appUserToken) {
        await _appUserTokenRepository.AddAsync(appUserToken);
    }

    public async Task UpdateAsync(AppUserToken appUserToken) {
        await _appUserTokenRepository.UpdateAsync(appUserToken);
    }

    public async Task DeleteAsync(AppUserToken appUserToken) {
        await _appUserTokenRepository.DeleteAsync(appUserToken);
    }

    public async Task<int> GetCountByUserIdAsync(Guid userId) {
        return await _appUserTokenRepository.GetCountByUserIdAsync(userId);
    }

    public async Task<AppUserToken> GetTokenByUserIdAsync(Guid userId) {
        return await _appUserTokenRepository.GetTokenByUserIdAsync(userId);
    }

    public async Task<TokenInfo> GenerateTokenAsync(AppUser appUser, List<string> appRoles) {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
        List<Claim> claims = new List<Claim>();

        foreach (var role in appRoles) {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        claims.Add(new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString()));
        claims.Add(new Claim(ClaimTypes.Name, appUser.UserName));
        claims.Add(new Claim(ClaimTypes.Email, appUser.Email));

        var tokenDescriptor = new SecurityTokenDescriptor {
            Audience = _jwtSettings.Audience,
            Issuer = _jwtSettings.Issuer,
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        TokenInfo tokenInfo = new TokenInfo();

        tokenInfo.Token = tokenString;
        tokenInfo.ExpireDate = tokenDescriptor.Expires.Value;

        return tokenInfo;
    }

    public async Task<AppUserToken> GetTokenAsync(AppUser appUser, List<string> appRoles) {
        AppUserToken userToken = null;
        TokenInfo tokenInfo = null;

        if (await GetCountByUserIdAsync(appUser.Id) > 0) {
            userToken = await GetTokenByUserIdAsync(appUser.Id);

            if (userToken.ExpireDate <= DateTime.UtcNow) {
                tokenInfo = await GenerateTokenAsync(appUser, appRoles);

                userToken.ExpireDate = tokenInfo.ExpireDate;
                userToken.Value = tokenInfo.Token;

                UpdateAsync(userToken);
            }
        }
        else {
            tokenInfo = await GenerateTokenAsync(appUser, appRoles);

            userToken = new AppUserToken();

            userToken.UserId = appUser.Id;
            userToken.LoginProvider = "LocalTest";
            userToken.Name = appUser.UserName;
            userToken.ExpireDate = tokenInfo.ExpireDate;
            userToken.Value = tokenInfo.Token;

            AddAsync(userToken);
        }
        return userToken;
    }
}

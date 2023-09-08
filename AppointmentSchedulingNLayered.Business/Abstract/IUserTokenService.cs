using AppointmentSchedulingNLayered.Entities.Concrete;
using AppointmentSchedulingNLayered.Common.Jwt;

namespace AppointmentSchedulingNLayered.Business.Abstract;

public interface IUserTokenService {
    Task<int> GetCountByUserIdAsync(Guid userId);
    Task<AppUserToken> GetTokenByUserIdAsync(Guid userId);
    Task UpdateAsync(AppUserToken appUserToken);
    Task DeleteAsync(AppUserToken appUserToken);
    Task AddAsync(AppUserToken appUserToken);
    Task<TokenInfo> GenerateTokenAsync(AppUser appUser, List<string> appRoles);
    Task<AppUserToken> GetTokenAsync(AppUser appUser, List<string> appRoles);
}
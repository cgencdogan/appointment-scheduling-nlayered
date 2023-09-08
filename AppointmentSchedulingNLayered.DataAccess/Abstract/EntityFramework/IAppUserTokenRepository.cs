using AppointmentSchedulingNLayered.Entities.Concrete;

namespace AppointmentSchedulingNLayered.DataAccess.Abstract.EntityFramework;
public interface IAppUserTokenRepository : IBaseRepository<AppUserToken> {
    Task<int> GetCountByUserIdAsync(Guid userId);
    Task<AppUserToken> GetTokenByUserIdAsync(Guid userId);
}

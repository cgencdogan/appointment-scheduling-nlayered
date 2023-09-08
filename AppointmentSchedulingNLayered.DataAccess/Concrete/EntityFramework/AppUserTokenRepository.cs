using AppointmentSchedulingNLayered.DataAccess.Abstract.EntityFramework;
using AppointmentSchedulingNLayered.DataAccess.Concrete.EntityFramework.Contexts;
using AppointmentSchedulingNLayered.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace AppointmentSchedulingNLayered.DataAccess.Concrete.EntityFramework;
internal class AppUserTokenRepository : EFBaseRepository<AppUserToken>, IAppUserTokenRepository {
    public AppUserTokenRepository(MsDbContext context) : base(context) { }

    public async Task<int> GetCountByUserIdAsync(Guid userId) {
        return await Context.Set<AppUserToken>().CountAsync(x => x.UserId == userId);
    }

    public async Task<AppUserToken> GetTokenByUserIdAsync(Guid userId) {
        return await Context.Set<AppUserToken>().FirstOrDefaultAsync(x => x.UserId == userId);
    }
}

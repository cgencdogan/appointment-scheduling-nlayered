using AppointmentSchedulingNLayered.DataAccess.Abstract.EntityFramework;
using AppointmentSchedulingNLayered.DataAccess.Concrete.EntityFramework;
using AppointmentSchedulingNLayered.DataAccess.Concrete.EntityFramework.Contexts;
using AppointmentSchedulingNLayered.Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppointmentSchedulingNLayered.DataAccess;
public static class DependencyInjection {
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration) {
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IAppUserTokenRepository, AppUserTokenRepository>();

        services.AddDbContext<MsDbContext>(options => {
            options.UseSqlServer(configuration.GetConnectionString("AppointmentSchedulingMS"));
        });

        services.AddIdentity<AppUser, AppRole>().AddRoleManager<RoleManager<AppRole>>().AddEntityFrameworkStores<MsDbContext>().AddDefaultTokenProviders();
        return services;
    }
}

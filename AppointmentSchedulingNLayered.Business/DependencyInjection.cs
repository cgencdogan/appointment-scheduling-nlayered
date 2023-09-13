using AppointmentSchedulingNLayered.Business.Abstract;
using AppointmentSchedulingNLayered.Business.Concrete;
using AppointmentSchedulingNLayered.Common.Jwt;
using AppointmentSchedulingNLayered.Common.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AppointmentSchedulingNLayered.Business;
public static class DependencyInjection {
    public static IServiceCollection AddBusiness(this IServiceCollection services, ConfigurationManager configuration) {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.Configure<RedisSettings>(configuration.GetSection("RedisSettings"));

        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IUserTokenService, UserTokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICacheService, RedisCacheService>();

        return services;
    }
}

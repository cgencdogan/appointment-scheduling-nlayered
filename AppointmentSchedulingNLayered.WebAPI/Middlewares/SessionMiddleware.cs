using AppointmentSchedulingNLayered.Common.Session;
using System.Security.Claims;

namespace AppointmentSchedulingNLayered.WebAPI.Middlewares;

public class SessionMiddleware {
    private readonly RequestDelegate _next;

    public SessionMiddleware(RequestDelegate next) {
        _next = next;
    }

    public async Task Invoke(HttpContext context, SessionProvider sessionProvider) {
        var user = context.User;

        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!string.IsNullOrEmpty(userId)) {
            sessionProvider.Initialise(userId);
        }
        await _next(context);
    }
}
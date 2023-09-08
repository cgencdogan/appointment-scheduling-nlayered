using AppointmentSchedulingNLayered.Common.Results;
using Newtonsoft.Json;
using System.Net;

namespace AppointmentSchedulingNLayered.WebAPI.Middlewares;

public class HttpStatusCodeMiddleware {
    private readonly RequestDelegate _next;

    public HttpStatusCodeMiddleware(RequestDelegate next) {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context) {
        await _next(context);
        if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized) {
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorDetails() {
                StatusCode = 401,
                Exception = "Yetki hatası."
            }));
        }
        if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden) {
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorDetails() {
                StatusCode = 403,
                Exception = "Yetki hatası."
            }));
        }
    }
}
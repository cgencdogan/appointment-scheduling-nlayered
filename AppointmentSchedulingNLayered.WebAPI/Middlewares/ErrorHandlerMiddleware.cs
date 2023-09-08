using AppointmentSchedulingNLayered.Common.Exceptions;
using AppointmentSchedulingNLayered.Common.Results;
using FluentValidation;
using Newtonsoft.Json;
using Serilog;
using System.Net;

namespace AppointmentSchedulingNLayered.WebAPI.Middlewares;

public class ExceptionMiddleware {
    private readonly RequestDelegate _next;
    private static readonly Serilog.ILogger _logger = Log.ForContext<ExceptionMiddleware>();

    public ExceptionMiddleware(RequestDelegate next) {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context) {
        try {
            await _next(context);
        }
        catch (Exception exception) {
            var errorResult = new ErrorDetails {
                Exception = exception.Message.Trim()
            };

            if (exception is not CustomException && exception.InnerException != null) {
                while (exception.InnerException != null) {
                    exception = exception.InnerException;
                }
            }

            switch (exception) {
                case CustomException e:
                    errorResult.StatusCode = (int)e.StatusCode;
                    if (e.ErrorMessages is not null) {
                        errorResult.Messages = e.ErrorMessages;
                    }
                    _logger.Error(e.Message);
                    break;

                case ValidationException e:
                    errorResult.Exception = "Bir veya birden fazla doğrulama hatası.";
                    errorResult.Messages.Clear();
                    foreach (var error in e.Errors) {
                        errorResult.Messages.Add(error.ErrorMessage);
                    }
                    errorResult.StatusCode = (int)HttpStatusCode.BadRequest;
                    _logger.Error(e.Message);
                    break;

                default:
                    errorResult.StatusCode = (int)HttpStatusCode.InternalServerError;
                    _logger.Error(exception, "Beklenmedik hata");
                    break;
            }

            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = errorResult.StatusCode;

            await response.WriteAsync(JsonConvert.SerializeObject(errorResult));
        }
    }
}

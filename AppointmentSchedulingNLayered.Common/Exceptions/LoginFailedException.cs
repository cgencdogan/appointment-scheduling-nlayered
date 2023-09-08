using System.Net;

namespace AppointmentSchedulingNLayered.Common.Exceptions;

public class LoginFailedException : CustomException {
    public LoginFailedException(
        string message = "Giriş başarısız.",
        List<string>? errors = null,
        HttpStatusCode statusCode = HttpStatusCode.Unauthorized)
        : base(message, errors, statusCode) {
    }
}

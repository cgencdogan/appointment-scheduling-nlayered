using System.Net;

namespace AppointmentSchedulingNLayered.Common.Exceptions;

public class RegisterFailedException : CustomException {
    public RegisterFailedException(
        string message = "Kayıt başarısız.",
        List<string>? errors = null,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        : base(message, errors, statusCode) {
    }
}
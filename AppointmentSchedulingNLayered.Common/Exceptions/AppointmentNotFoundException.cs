using System.Net;

namespace AppointmentSchedulingNLayered.Common.Exceptions;
public class AppointmentNotFoundException : CustomException {
    public AppointmentNotFoundException(
        string message = "Randevu bulunamadı.",
        List<string>? errors = null,
        HttpStatusCode statusCode = HttpStatusCode.NotFound)
        : base(message, errors, statusCode) {
    }
}

using System.Net;

namespace AppointmentSchedulingNLayered.Common.Exceptions;

public class AppointmentTimeNotAvailableException : CustomException {
    public AppointmentTimeNotAvailableException(
        string message = "Randevu tarihi mevcut randevular ile çakışıyor.",
        List<string>? errors = null,
        HttpStatusCode statusCode = HttpStatusCode.Conflict)
        : base(message, errors, statusCode) {
    }
}
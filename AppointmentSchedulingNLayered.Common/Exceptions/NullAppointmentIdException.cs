using System.Net;

namespace AppointmentSchedulingNLayered.Common.Exceptions;

public class NullAppointmentIdException : CustomException {
    public NullAppointmentIdException(
        string message = "Randevu ID boş olamaz.",
        List<string>? errors = null,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        : base(message, errors, statusCode) {
    }
}

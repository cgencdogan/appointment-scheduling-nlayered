using AppointmentSchedulingNLayered.Entities.Concrete;

namespace AppointmentSchedulingNLayered.DataAccess.Abstract.EntityFramework;
public interface IAppointmentRepository : IBaseRepository<Appointment> {
    Task<bool> AppointmentExistsAsync(Guid appointmentId);
    Task<bool> AppointmentTimeAvailableAsync(Appointment appointment);
}

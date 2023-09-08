using AppointmentSchedulingNLayered.DataAccess.Abstract.EntityFramework;
using AppointmentSchedulingNLayered.DataAccess.Concrete.EntityFramework.Contexts;
using AppointmentSchedulingNLayered.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace AppointmentSchedulingNLayered.DataAccess.Concrete.EntityFramework;
public class AppointmentRepository : EFBaseRepository<Appointment>, IAppointmentRepository {
    public AppointmentRepository(MsDbContext context) : base(context) { }

    public async Task<bool> AppointmentExistsAsync(Guid appointmentId) {
        return await Context.Set<Appointment>().AnyAsync(a => a.Id == appointmentId);
    }

    public async Task<bool> AppointmentTimeAvailableAsync(Appointment appointment) {
        return !(await Context.Set<Appointment>().AnyAsync(a => a.EndDate > appointment.StartDate && a.StartDate < appointment.EndDate));
    }
}

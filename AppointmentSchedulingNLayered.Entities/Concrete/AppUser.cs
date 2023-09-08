using Microsoft.AspNetCore.Identity;

namespace AppointmentSchedulingNLayered.Entities.Concrete;
public class AppUser : IdentityUser<Guid> {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime CreatedDate { get; set; }
    public List<Appointment> Appointments { get; set; }
    public List<Appointment> CreatedAppointments { get; set; }
}
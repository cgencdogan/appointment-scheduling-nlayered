using AppointmentSchedulingNLayered.Entities.Abstract;

namespace AppointmentSchedulingNLayered.Entities.Concrete;

public class Appointment : BaseEntity {
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Details { get; set; }
    public Guid CreatedById { get; set; }
    public AppUser CreatedBy { get; set; }
    public Guid PersonnelId { get; set; }
    public AppUser Personnel { get; set; }
}
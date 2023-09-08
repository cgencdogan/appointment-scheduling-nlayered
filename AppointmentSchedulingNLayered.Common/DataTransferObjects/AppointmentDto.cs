namespace AppointmentSchedulingNLayered.Common.DataTransferObjects;
public class AddAppointmentDto : IAppointmentDto {
    public Guid PersonnelId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Details { get; set; }
}

public class UpdateAppointmentDto : IAppointmentDto {
    public Guid Id { get; set; }
    public Guid PersonnelId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Details { get; set; }
}
public class AppointmentDto : IAppointmentDto {
    public Guid Id { get; set; }
    public Guid PersonnelId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Details { get; set; }
}
public interface IAppointmentDto {
    public Guid PersonnelId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Details { get; set; }
}
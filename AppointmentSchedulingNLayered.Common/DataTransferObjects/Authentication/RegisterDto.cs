namespace AppointmentSchedulingNLayered.Common.DataTransferObjects.Authentication;

public class RegisterDto {
    public string UserName { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
}

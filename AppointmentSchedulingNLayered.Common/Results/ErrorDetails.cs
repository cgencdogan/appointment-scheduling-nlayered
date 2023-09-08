namespace AppointmentSchedulingNLayered.Common.Results;

public class ErrorDetails {
    public int StatusCode { get; set; }
    public string? Exception { get; set; }
    public List<string> Messages { get; set; } = new();
}
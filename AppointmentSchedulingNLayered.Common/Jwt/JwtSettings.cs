namespace AppointmentSchedulingNLayered.Common.Jwt;

public class JwtSettings {
    public string Secret { get; set; }
    public int ExpiryMinutes { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public static string SectionName { get; set; } = "JwtSettings";
}
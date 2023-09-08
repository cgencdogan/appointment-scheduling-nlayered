namespace AppointmentSchedulingNLayered.Common.Session;

public class SessionProvider {
    public virtual Session Session { get; set; }

    public SessionProvider() {
        Session = new Session();
    }

    public void Initialise(string userId) {
        Session.UserId = userId;
    }
}
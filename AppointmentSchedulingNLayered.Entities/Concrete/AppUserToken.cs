using AppointmentSchedulingNLayered.Entities.Abstract;
using Microsoft.AspNetCore.Identity;

namespace AppointmentSchedulingNLayered.Entities.Concrete;

public class AppUserToken : IdentityUserToken<Guid>, IEntity {
    public DateTime ExpireDate { get; set; }
}

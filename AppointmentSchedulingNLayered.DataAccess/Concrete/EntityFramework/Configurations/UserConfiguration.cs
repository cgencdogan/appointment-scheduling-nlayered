using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using AppointmentSchedulingNLayered.Entities.Concrete;

namespace AppointmentSchedulingNLayered.DataAccess.Concrete.EntityFramework.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<AppUser> {
    public void Configure(EntityTypeBuilder<AppUser> builder) {
        var hasher = new PasswordHasher<AppUser>();
        builder.HasData(
             new AppUser {
                 Id = new Guid("9e1ba931-e5eb-4172-9dde-ae3270947d3c"),
                 Email = "caner@admin.com",
                 NormalizedEmail = "CANER@ADMIN.COM",
                 FirstName = "Caner",
                 LastName = "Admin",
                 UserName = "CanerAdmin",
                 NormalizedUserName = "CANERADMIN",
                 PasswordHash = hasher.HashPassword(null, "Caner_123"),
                 SecurityStamp = Guid.NewGuid().ToString("N").ToUpper(),
                 EmailConfirmed = false,
                 CreatedDate = DateTime.UtcNow
             }
        );
    }
}

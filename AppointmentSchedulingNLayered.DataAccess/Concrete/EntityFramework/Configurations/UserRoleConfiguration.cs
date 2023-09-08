using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AppointmentSchedulingNLayered.DataAccess.Concrete.EntityFramework.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<Guid>> {
    public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> builder) {
        builder.HasData(
            new IdentityUserRole<Guid> {
                RoleId = new Guid("9fac9280-2c55-48e5-a21f-f2ec25f9e046"),
                UserId = new Guid("9e1ba931-e5eb-4172-9dde-ae3270947d3c")
            });
    }
}
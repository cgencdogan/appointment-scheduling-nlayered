using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using AppointmentSchedulingNLayered.Entities.Concrete;

namespace AppointmentSchedulingNLayered.DataAccess.Concrete.EntityFramework.Configurations;
public class RoleConfiguration : IEntityTypeConfiguration<AppRole> {
    public void Configure(EntityTypeBuilder<AppRole> builder) {
        builder.HasData(
            new AppRole {
                Id = new Guid("9fac9280-2c55-48e5-a21f-f2ec25f9e046"),
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
             new AppRole {
                 Id = new Guid("73849f62-58fa-4cf7-8bd2-8e2a9389ded0"),
                 Name = "User",
                 NormalizedName = "USER"
             }
        );
    }
}

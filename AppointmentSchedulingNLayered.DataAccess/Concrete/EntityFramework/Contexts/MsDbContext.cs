using AppointmentSchedulingNLayered.DataAccess.Concrete.EntityFramework.Configurations;
using AppointmentSchedulingNLayered.Entities.Concrete;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppointmentSchedulingNLayered.DataAccess.Concrete.EntityFramework.Contexts;
public class MsDbContext : IdentityDbContext<AppUser, AppRole, Guid> {
    public MsDbContext(DbContextOptions<MsDbContext> options) : base(options) { }

    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<AppUserToken> AppUserTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoleConfiguration());

        modelBuilder.Entity<Appointment>(entity => {
            entity.HasOne(a => a.CreatedBy)
            .WithMany(u => u.CreatedAppointments)
            .HasForeignKey(a => a.CreatedById)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FK_Appointment_AspNetUsers_CreatedBy");

            entity.HasOne(a => a.Personnel)
                .WithMany(u => u.Appointments)
                .HasForeignKey(a => a.PersonnelId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Appointment_AspNetUsers_Personnel");
        });
    }
}
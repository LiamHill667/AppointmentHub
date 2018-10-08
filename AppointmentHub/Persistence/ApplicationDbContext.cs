using AppointmentHub.Core.Models;
using AppointmentHub.Persistence.EntityConfigurations;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace AppointmentHub.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole,
        string, IdentityUserLogin, ApplicationUserRole, IdentityUserClaim>, IApplicationDbContext
    {

        public DbSet<UserAvailability> UserAvailabilties { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentType> AppointmentTypes { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ApplicationUserConfiguration());
            modelBuilder.Configurations.Add(new AppointmentConfiguration());
            modelBuilder.Configurations.Add(new UserAvailabilityConfiguration());
            modelBuilder.Configurations.Add(new NotificationConfiguration());
            modelBuilder.Configurations.Add(new UserNotificationConfiguration());
            modelBuilder.Configurations.Add(new ApplicationRoleConfiguration());
            modelBuilder.Configurations.Add(new ApplicationUserRoleConfiguration());

            base.OnModelCreating(modelBuilder);
        }


        public EntityState GetEntityState(object entity)
        {
            return Entry(entity).State;
        }

        public void SetEntityModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }
    }
}
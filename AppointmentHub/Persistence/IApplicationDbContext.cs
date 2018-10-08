using AppointmentHub.Core.Models;
using System;
using System.Data.Entity;

namespace AppointmentHub.Persistence
{
    public interface IApplicationDbContext
    {
        DbSet<UserAvailability> UserAvailabilties { get; set; }
        DbSet<Appointment> Appointments { get; set; }
        DbSet<AppointmentType> AppointmentTypes { get; set; }
        DbSet<Notification> Notifications { get; set; }
        DbSet<UserNotification> UserNotifications { get; set; }
        IDbSet<ApplicationUser> Users { get; set; }
        IDbSet<ApplicationRole> Roles { get; set; }

        EntityState GetEntityState(object entity);
        void SetEntityModified(object entity);
        DbSet Set(Type entityType);
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

    }

}
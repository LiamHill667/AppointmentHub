using AppointmentHub.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace AppointmentHub.Persistence.EntityConfigurations
{
    public class NotificationConfiguration : EntityTypeConfiguration<Notification>
    {
        public NotificationConfiguration()
        {
            HasRequired(n => n.Appointment);
        }
    }
}
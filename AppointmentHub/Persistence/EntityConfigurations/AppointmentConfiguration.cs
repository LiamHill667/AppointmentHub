using AppointmentHub.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace AppointmentHub.Persistence.EntityConfigurations
{
    public class AppointmentConfiguration : EntityTypeConfiguration<Appointment>
    {
        public AppointmentConfiguration()
        {
            Property(a => a.RequestedId)
                .IsRequired();

            Property(a => a.RequesteeId)
                .IsRequired();

            Property(a => a.Subject)
                .IsRequired()
                .HasMaxLength(255);

            Property(a => a.TypeId)
                .IsRequired();
        }
    }

    public class AppointmentTypeConfiguration : EntityTypeConfiguration<AppointmentType>
    {
        public AppointmentTypeConfiguration()
        {
            HasKey(at => new { at.Id, at.Name });

            Property(at => at.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
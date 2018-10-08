using AppointmentHub.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace AppointmentHub.Persistence.EntityConfigurations
{
    public class ApplicationUserConfiguration : EntityTypeConfiguration<ApplicationUser>
    {
        public ApplicationUserConfiguration()
        {
            Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            HasMany(u => u.RequestedAppointments)
                 .WithRequired(a => a.Requestee)
                 .HasForeignKey(a => a.RequesteeId)
                 .WillCascadeOnDelete(false);

            HasMany(u => u.InvitedAppointments)
                .WithRequired(a => a.Requested)
                .HasForeignKey(a => a.RequestedId)
                .WillCascadeOnDelete(false);
            

        }
    }
}
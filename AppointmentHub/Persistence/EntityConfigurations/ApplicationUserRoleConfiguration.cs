using AppointmentHub.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace AppointmentHub.Persistence.EntityConfigurations
{
    public class ApplicationUserRoleConfiguration : EntityTypeConfiguration<ApplicationUserRole>
    {
        public ApplicationUserRoleConfiguration()
        {
            HasRequired(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(r => r.RoleId)
                .WillCascadeOnDelete(true);


        }
    }
}
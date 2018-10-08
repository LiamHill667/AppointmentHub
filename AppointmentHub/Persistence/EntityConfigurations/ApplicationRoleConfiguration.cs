using AppointmentHub.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace AppointmentHub.Persistence.EntityConfigurations
{
    public class ApplicationRoleConfiguration : EntityTypeConfiguration<ApplicationRole>
    {
        public ApplicationRoleConfiguration()
        {
            HasMany(u => u.UserRoles)
             .WithRequired(ur => ur.Role)
             .HasForeignKey(ur => ur.RoleId)
             .WillCascadeOnDelete(true);

        }
    }
}
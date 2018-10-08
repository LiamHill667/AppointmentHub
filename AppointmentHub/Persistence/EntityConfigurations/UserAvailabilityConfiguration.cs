using AppointmentHub.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace AppointmentHub.Persistence.EntityConfigurations
{
    public class UserAvailabilityConfiguration : EntityTypeConfiguration<UserAvailability>
    {
        public UserAvailabilityConfiguration()
        {

        }
    }
}
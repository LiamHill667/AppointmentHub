using AppointmentHub.Core.Models;
using AppointmentHub.Core.Repositories;

namespace AppointmentHub.Persistence.Repositories
{
    public class AppointmentTypeRepository : GenericRepository<AppointmentType>, IAppointmentTypeRepository
    {
        public AppointmentTypeRepository(IApplicationDbContext context) : base(context)
        {
        }
    }
}
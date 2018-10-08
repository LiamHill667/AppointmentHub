using AppointmentHub.Core.Models;
using System.Collections.Generic;

namespace AppointmentHub.Core.Repositories
{
    public interface IApplicationUserRepository : IGenericRepository<ApplicationUser>
    {
        IEnumerable<ApplicationUser> GetPagedUsers(int pageNum, out int totalPages, string query = null, int pageSize = 12);
    }
}

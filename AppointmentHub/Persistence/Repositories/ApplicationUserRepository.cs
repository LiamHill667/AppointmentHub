using AppointmentHub.Core.Models;
using AppointmentHub.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AppointmentHub.Persistence.Repositories
{
    public class ApplicationUserRepository : GenericRepository<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(IApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<ApplicationUser> GetPagedUsers(int pageNum, out int totalPages, string query = null, int pageSize = 12)
        {
            var users = _context.Users
                .Include(u => u.Roles.Select(r => r.Role));

            if (!string.IsNullOrEmpty(query))
            {
                users = users
                    .Where(u => u.Name == query);
            }

            totalPages = users.Count() / pageSize;

            return users.OrderBy(a => a.Name).Skip(pageNum * pageSize).Take(pageSize).ToList();
        }
    }
}
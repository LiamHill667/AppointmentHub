using AppointmentHub.Core.Models;
using AppointmentHub.Core.Repositories;
using AppointmentHub.Persistence.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AppointmentHub.Persistence.Repositories
{
    public class UserAvailabilityRepository : GenericRepository<UserAvailability>, IUserAvailabilityRepository
    {
        public UserAvailabilityRepository(IApplicationDbContext context) : base(context)
        {

        }

        public UserAvailability GetById(int Id)
        {
            return _context.UserAvailabilties
                .Include(a => a.User)
                .SingleOrDefault(a => a.Id == Id);
        }

        public IEnumerable<UserAvailability> GetFutureUserAvailabilities(string userId)
        {
            return _context.UserAvailabilties
                .Where(a => a.UserId == userId && a.DateTime > DateTime.Now)
                .ToList();
        }

        public IEnumerable<UserAvailability> GetFutureUserAvailabilitiesPaged(string userId, int pageNum, out int totalPages, string searchTerm = null, int pageSize = 12)
        {
            var availabilities = _context.UserAvailabilties
                .Where(a => a.UserId == userId && a.DateTime > DateTime.Now);

            if (!String.IsNullOrWhiteSpace(searchTerm))
            {
                var parsed = DateTime.TryParse(searchTerm, out DateTime parsedDate);

                if (!parsed)
                    throw (new FormatException("searchTerm string is not in parsable datetime format"));

                availabilities = availabilities
                    .Where(a => a.DateTime.TruncateTime() == parsedDate.TruncateTime());
            }
            //
            totalPages = 0;

            return GetPaged(availabilities, pageNum, pageSize, ref totalPages, searchTerm);
        }

        public IEnumerable<UserAvailability> GetFutureBookableUserAvailabilities(string requesteeId)
        {
            return _context.UserAvailabilties
                .Where(a => a.UserId != requesteeId && a.DateTime > DateTime.Now)
                .Include(a => a.User)
                .ToList();
        }

        public IEnumerable<UserAvailability> GetFutureBookableUserAvailabilitiesPaged(string requesteeId, int pageNum, out int totalPages, string searchTerm = null, int pageSize = 12)
        {
            var availabilities = _context.UserAvailabilties
                .Where(a => a.UserId != requesteeId && a.DateTime > DateTime.Now)
                .Include(a => a.User);

            if (!String.IsNullOrWhiteSpace(searchTerm))
            {
                var parsed = DateTime.TryParse(searchTerm, out DateTime parsedDate);

                if (!parsed)
                    throw (new FormatException("searchTerm string is not in parsable datetime format"));

                availabilities = availabilities
                    .Where(a => a.DateTime.TruncateTime() == parsedDate.TruncateTime());
            }

            totalPages = 0;

            return GetPaged(availabilities, pageNum, pageSize, ref totalPages, searchTerm);
        }

        public IEnumerable<UserAvailability> GetPaged(IQueryable<UserAvailability> availabilities, int pageNum, int pageSize, ref int totalPages, string searchTerm = null)
        {
            totalPages = availabilities.Count() / pageSize;

            return availabilities.OrderBy(a => a.DateTime).Skip(pageNum * pageSize).Take(pageSize).ToList();
        }
    }
}
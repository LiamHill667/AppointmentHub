using AppointmentHub.Core.Models;
using System.Collections.Generic;

namespace AppointmentHub.Core.Repositories
{
    public interface IUserAvailabilityRepository : IGenericRepository<UserAvailability>
    {
        UserAvailability GetById(int id);
        IEnumerable<UserAvailability> GetFutureUserAvailabilities(string userId);
        IEnumerable<UserAvailability> GetFutureBookableUserAvailabilities(string requesteeId);
        IEnumerable<UserAvailability> GetFutureUserAvailabilitiesPaged(string userId, int pageNum, out int totalPages, string searchTerm = null, int pageSize = 12);
        IEnumerable<UserAvailability> GetFutureBookableUserAvailabilitiesPaged(string requesteeId, int pageNum, out int totalPages, string searchTerm = null, int pageSize = 12);
    }
}

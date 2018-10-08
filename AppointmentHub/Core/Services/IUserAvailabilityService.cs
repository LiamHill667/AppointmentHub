using AppointmentHub.Core.Models;
using System.Collections.Generic;

namespace AppointmentHub.Core.Services
{
    public interface IUserAvailabilityService
    {
        IEnumerable<UserAvailability> GetFutureUserAvailabilitiesPaged(string userId, int pageNum, out int totalPages, string query);
        IEnumerable<UserAvailability> GetFutureBookableAvailabilitiesPaged(string userId, int pageNum, out int totalPages, string query);
        ServiceResult CreateAvailability(UserAvailability userAvailability);
        ServiceResult UpdateAvailability(UserAvailability userAvailability);
        ServiceResult DeleteAvailability(int id, string userId);
        UserAvailability GetAvailability(int id);

    }
}

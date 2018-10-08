using AppointmentHub.Core.Models;
using AppointmentHub.Core.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace AppointmentHub.Persistence.Repositories
{
    public class UserNotificationRepository : GenericRepository<UserNotification>, IUserNotificationRepository
    {

        public UserNotificationRepository(IApplicationDbContext context) : base(context)
        {

        }

        public IEnumerable<UserNotification> GetUserNotificationsFor(string userId)
        {
            return _context.UserNotifications
                .Where(un => un.UserId == userId && !un.IsRead)
                .ToList();
        }
    }
}
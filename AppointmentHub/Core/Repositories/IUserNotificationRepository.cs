using AppointmentHub.Core.Models;
using System.Collections.Generic;

namespace AppointmentHub.Core.Repositories
{
    public interface IUserNotificationRepository : IGenericRepository<UserNotification>
    {
        IEnumerable<UserNotification> GetUserNotificationsFor(string userId);
    }
}
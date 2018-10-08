using AppointmentHub.Core.Models;
using System.Collections.Generic;

namespace AppointmentHub.Core.Repositories
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        IEnumerable<Notification> GetNewNotificationsFor(string userId);
    }
}
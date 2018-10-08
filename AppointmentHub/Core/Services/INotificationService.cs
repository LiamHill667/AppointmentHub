using AppointmentHub.Core.Models;
using System.Collections.Generic;

namespace AppointmentHub.Core.Services
{
    public interface INotificationService
    {
        IEnumerable<Notification> GetNewNotifications(string userId);
        ServiceResult MarkAsRead(string userId);
    }
}

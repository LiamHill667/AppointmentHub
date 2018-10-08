using AppointmentHub.Core.Models;
using Microsoft.Ajax.Utilities;
using System.Collections.Generic;

namespace AppointmentHub.Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Notification> GetNewNotifications(string userId)
        {
            return _unitOfWork.Notifications.GetNewNotificationsFor(userId);
        }

        public ServiceResult MarkAsRead(string userId)
        {
            var notifications = _unitOfWork.UserNotifications.GetUserNotificationsFor(userId);

            notifications.ForEach(n => n.Read());

            _unitOfWork.Complete();

            return new ServiceResult(true);
        }
    }
}
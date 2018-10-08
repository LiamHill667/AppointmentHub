using AppointmentHub.Core.Models;
using AppointmentHub.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AppointmentHub.Persistence.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {

        public NotificationRepository(IApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<Notification> GetNewNotificationsFor(string userId)
        {
            return _context.UserNotifications
                .Where(un => un.UserId == userId && !un.IsRead)
                .Select(un => un.Notification)
                .Include(n => n.Appointment)
                .Include(n => n.Appointment.Requested)
                .Include(n => n.Appointment.Requestee)
                .Include(n => n.Appointment.Type)
                .ToList();
        }
    }
}
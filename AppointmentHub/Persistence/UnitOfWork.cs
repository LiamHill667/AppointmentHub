using AppointmentHub.Core;
using AppointmentHub.Core.Repositories;
using AppointmentHub.Persistence.Repositories;

namespace AppointmentHub.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IUserAvailabilityRepository UserAvailability { get; private set; }

        public IAppointmentRepository Appointment { get; private set; }

        public IAppointmentTypeRepository AppointmentType { get; private set; }

        public INotificationRepository Notifications { get; private set; }

        public IUserNotificationRepository UserNotifications { get; private set; }

        public IApplicationUserRepository ApplicationUsers { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            UserAvailability = new UserAvailabilityRepository(context);
            Appointment = new AppointmentRepository(context);
            AppointmentType = new AppointmentTypeRepository(context);
            Notifications = new NotificationRepository(context);
            UserNotifications = new UserNotificationRepository(context);
            ApplicationUsers = new ApplicationUserRepository(context);
        }

        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}
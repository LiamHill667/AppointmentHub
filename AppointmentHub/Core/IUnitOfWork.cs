using AppointmentHub.Core.Repositories;

namespace AppointmentHub.Core
{
    public interface IUnitOfWork
    {
        IUserAvailabilityRepository UserAvailability { get; }
        IAppointmentRepository Appointment { get; }
        IAppointmentTypeRepository AppointmentType { get; }
        INotificationRepository Notifications { get; }
        IUserNotificationRepository UserNotifications { get; }
        IApplicationUserRepository ApplicationUsers { get; }
        void Complete();
    }
}

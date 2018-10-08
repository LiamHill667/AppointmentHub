using AppointmentHub.Core.Common;
using System;

namespace AppointmentHub.Core.ViewModels
{
    public class NotificationViewModel
    {
        public DateTime DateTime { get; set; }
        public NotificationType Type { get; set; }
        public DateTime? OriginalDateTime { get; set; }
        public AppointmentViewModel Appointment { get; set; }
    }
}
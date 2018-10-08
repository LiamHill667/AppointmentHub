using AppointmentHub.Core.Common;
using System;

namespace AppointmentHub.Core.Models
{
    public class Notification
    {
        public int Id { get; private set; }
        public DateTime DateTime { get; private set; }
        public NotificationType Type { get; private set; }
        public DateTime? OriginalDateTime { get; private set; }

        public Appointment Appointment { get; private set; }

        protected Notification()
        {
        }

        private Notification(NotificationType type, Appointment appointment)
        {
            if (appointment == null)
                throw new ArgumentNullException("Appointment");

            Type = type;
            Appointment = appointment;
            DateTime = DateTime.Now;
        }

        public static Notification AppointmentCreated(Appointment appointment)
        {
            return new Notification(NotificationType.AppointmentCreated, appointment);
        }

        public static Notification ApppointmentCanceled(Appointment appointment)
        {
            return new Notification(NotificationType.AppointmentCanceled, appointment);
        }
    }
}
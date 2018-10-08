using System;

namespace AppointmentHub.Core.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public TimeSpan TimeSpan { get; set; }
        public string Subject { get; set; }
        public bool IsCanceled { get; set; }

        public int TypeId { get; set; }
        public AppointmentType Type { get; set; }

        public string RequestedId { get; set; }
        public ApplicationUser Requested { get; set; }

        public string RequesteeId { get; set; }
        public ApplicationUser Requestee { get; set; }


        public Appointment()
        {
            DateTime = DateTime.Now;
        }

        public void Cancel()
        {
            if (Requested == null)
                throw new ArgumentNullException("Requested");

            if (Requestee == null)
                throw new ArgumentNullException("Requestee");

            IsCanceled = true;

            var notification = Notification.ApppointmentCanceled(this);

            Requested.Notify(notification);
            Requestee.Notify(notification);
        }

        public void Booked()
        {
            if (Requested == null)
                throw new ArgumentNullException("Requested");

            if (Requestee == null)
                throw new ArgumentNullException("Requestee");

            var notification = Notification.AppointmentCreated(this);

            Requested.Notify(notification);
            Requestee.Notify(notification);
        }
    }
}
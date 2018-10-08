using System;

namespace AppointmentHub.Core.Models
{

    public class UserAvailability
    {
        public int Id { get; set; }

        public DateTime DateTime { get; set; }

        public TimeSpan TimeSpan { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public TimeSpan GetStartTime()
        {
            return new TimeSpan(DateTime.Hour, 0, 0);
        }

        public TimeSpan GetEndTime()
        {
            return new TimeSpan(DateTime.Add(TimeSpan).TimeOfDay.Hours, 0, 0);
        }

    }
}
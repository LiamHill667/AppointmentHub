using System;

namespace AppointmentHub.Core.ViewModels
{
    public class UserAvailabilityViewModel
    {
        public int Id { get; set; }

        public DateTime DateTime { get; set; }

        public TimeSpan TimeSpan { get; set; }

        public ApplicationUserViewModel User { get; set; }

        public string UserName { get; set; }
    }
}
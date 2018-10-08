using System;

namespace AppointmentHub.Core.ViewModels
{
    public class AppointmentViewModel
    {
        public int Id { get; set; }

        public DateTime DateTime { get; set; }

        public TimeSpan TimeSpan { get; set; }

        public string Subject { get; set; }

        public bool IsCanceled { get; set; }

        public AppointmentTypeViewModel Type { get; set; }

        public ApplicationUserViewModel Requested { get; set; }

        public ApplicationUserViewModel Requestee { get; set; }
    }
}
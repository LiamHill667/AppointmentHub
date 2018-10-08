using System.Collections.Generic;

namespace AppointmentHub.Core.ViewModels
{
    public class AvailabilityViewModel : PaginationViewModel
    {
        public IEnumerable<UserAvailabilityViewModel> Availability { get; set; }

    }
}
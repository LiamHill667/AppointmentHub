using System.Collections.Generic;

namespace AppointmentHub.Core.ViewModels
{
    public class AppointmentListViewModel : PaginationViewModel
    {
        public IEnumerable<AppointmentViewModel> Appointments { get; set; }
    }
}
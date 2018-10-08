using System.Collections.Generic;

namespace AppointmentHub.Core.ViewModels
{
    public class UsersViewModel : PaginationViewModel
    {
        public IEnumerable<ApplicationUserViewModel> Users { get; set; }

    }
}
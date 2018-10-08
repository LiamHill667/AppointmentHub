namespace AppointmentHub.Core.ViewModels
{
    public abstract class PaginationViewModel
    {
        public string SearchTerm { get; set; }

        public int PageNum { get; set; }

        public int TotalPages { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

    }
}
using System.ComponentModel.DataAnnotations;

namespace AppointmentHub.Core.ViewModels
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}

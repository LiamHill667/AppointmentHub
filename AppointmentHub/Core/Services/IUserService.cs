using AppointmentHub.Core.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;

namespace AppointmentHub.Core.Services
{
    public interface IUserService
    {
        IEnumerable<ApplicationUser> GetUsersPaged(int pageNum, out int totalPages, string query);
        ApplicationUser GetUser(string id);
        IdentityResult CreateUser(string name, string email, string password, IEnumerable<string> Roles, ApplicationUserManager userManager);
        ServiceResult UpdateUser(string id, string name, string email, IEnumerable<string> Roles, ApplicationUserManager userManager);
    }
}

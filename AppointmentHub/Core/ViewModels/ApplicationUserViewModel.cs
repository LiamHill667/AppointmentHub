using System.Collections.Generic;

namespace AppointmentHub.Core.ViewModels
{
    public class ApplicationUserViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<ApplicationUserRoleViewModel> Roles { get; set; }

        public string RolesToString()
        {
            string roles = "";

            foreach (var role in Roles)
            {
                roles += $"{role.Role.Name} ";
            }

            return roles.TrimEnd();
        }
    }
}
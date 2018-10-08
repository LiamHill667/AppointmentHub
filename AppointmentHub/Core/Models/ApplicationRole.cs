using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AppointmentHub.Core.Models
{
    public class ApplicationRole : IdentityRole<string, ApplicationUserRole>
    {

        public ICollection<ApplicationUserRole> UserRoles { get; set; }

        public ApplicationRole()
        {
            UserRoles = new Collection<ApplicationUserRole>();

            Id = Guid.NewGuid().ToString();
        }

        public ApplicationRole(string name) : this()
        {
            Name = name;
        }
    }
}
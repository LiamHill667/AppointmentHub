using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AppointmentHub.Core.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser
        : IdentityUser<string, IdentityUserLogin,
        ApplicationUserRole, IdentityUserClaim>
    {
        public string Name { get; set; }

        public ICollection<Appointment> RequestedAppointments { get; set; }
        public ICollection<Appointment> InvitedAppointments { get; set; }
        public ICollection<UserNotification> UserNotifications { get; set; }

        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            RequestedAppointments = new Collection<Appointment>();
            InvitedAppointments = new Collection<Appointment>();
            UserNotifications = new Collection<UserNotification>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public void Notify(Notification notification)
        {
            if (notification != null)
                UserNotifications.Add(new UserNotification(this, notification));
        }
    }
}
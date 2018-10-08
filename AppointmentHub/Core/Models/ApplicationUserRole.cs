using AppointmentHub.Persistence;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace AppointmentHub.Core.Models
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public virtual ApplicationRole Role { get; set; }

        public ApplicationUserRole()
        {
        }
    }


    public class ApplicationUserStore
        : UserStore<ApplicationUser, ApplicationRole, string,
            IdentityUserLogin, ApplicationUserRole,
            IdentityUserClaim>, IUserStore<ApplicationUser, string>,
        IDisposable
    {

        public ApplicationUserStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }


    public class ApplicationRoleStore
    : RoleStore<ApplicationRole, string, ApplicationUserRole>,
    IQueryableRoleStore<ApplicationRole, string>,
    IRoleStore<ApplicationRole, string>, IDisposable
    {
        public ApplicationRoleStore()
            : base(new IdentityDbContext())
        {
            base.DisposeContext = true;
        }

        public ApplicationRoleStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
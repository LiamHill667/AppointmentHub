namespace AppointmentHub.Migrations
{
    using AppointmentHub.Core.Common;
    using AppointmentHub.Core.Models;
    using AppointmentHub.Persistence;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Persistence\Migrations";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser, ApplicationRole, string, IdentityUserLogin, ApplicationUserRole, IdentityUserClaim>(context));
            var roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole, string, ApplicationUserRole>(context));

            const string name = "admin@example.com";
            const string password = "Admin@123456";

            //Create Role Admin if it does not exist
            var role = roleManager.FindByName(AppRoles.Admin);
            if (role == null)
            {
                role = new ApplicationRole(AppRoles.Admin);
                var roleresult = roleManager.Create(role);
            }

            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser { UserName = name, Email = name };
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            // Add user admin to Role Admin if not already added
            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                var result = userManager.AddToRole(user.Id, role.Name);
            }

            foreach (var appRole in AppRoles.GetRoleNames())
            {
                if (roleManager.FindByName(appRole) == null)
                {
                    var roleresult = roleManager
                        .Create(new ApplicationRole(appRole));
                }
            }
        }
    }
}

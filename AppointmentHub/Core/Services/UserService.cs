using AppointmentHub.Core.Common;
using AppointmentHub.Core.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;

namespace AppointmentHub.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IdentityResult CreateUser(string name, string email, string password, IEnumerable<string> Roles, ApplicationUserManager userManager)
        {
            var newUser = new ApplicationUser()
            {
                Name = name,
                UserName = email,
                Email = email,
            };

            var result = userManager.Create(newUser, password);

            if (result.Succeeded)
                ManageUserRoles(newUser.Id, Roles, userManager);

            return result;
        }

        public ApplicationUser GetUser(string id)
        {
            return _unitOfWork.ApplicationUsers.GetById(id);
        }

        public IEnumerable<ApplicationUser> GetUsersPaged(int pageNum, out int totalPages, string query)
        {
            return _unitOfWork.ApplicationUsers
                .GetPagedUsers(pageNum, out totalPages, query);
        }

        public ServiceResult UpdateUser(string id, string name, string email, IEnumerable<string> Roles, ApplicationUserManager userManager)
        {
            var user = GetUser(id);

            if (user == null)
                return new ServiceResult(false, "User not found");

            user.Name = name;
            user.Email = email;
            user.UserName = email;

            //var result = userManager.Update(user);

            //if (result.Succeeded)
            _unitOfWork.Complete();

            ManageUserRoles(user.Id, Roles, userManager);

            return new ServiceResult(true);
        }

        private void ManageUserRoles(string userId, IEnumerable<string> Roles, ApplicationUserManager userManager)
        {
            foreach (var role in AppRoles.GetRoleNames())
            {
                if (!Roles.Contains(role) && userManager.IsInRole(userId, role))
                {
                    userManager.RemoveFromRole(userId, role);
                }
                else if (Roles.Contains(role))
                {
                    userManager.AddToRole(userId, role);
                }
            }
        }
    }
}
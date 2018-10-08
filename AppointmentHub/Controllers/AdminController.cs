using AppointmentHub.Core.Common;
using AppointmentHub.Core.Services;
using AppointmentHub.Core.ViewModels;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AppointmentHub.Controllers
{
    [Authorize(Roles = AppRoles.Admin)]
    public class AdminController : Controller
    {
        private ApplicationUserManager _userManger;

        private readonly IUserService _userService;

        public AdminController(IUserService userService, ApplicationUserManager userManager)
        {
            _userService = userService;
            _userManger = userManager;
        }

        // GET: Admin
        public ActionResult Users(int pageNum = 0, string query = null)
        {
            var users = _userService
                .GetUsersPaged(pageNum, out int totalPages, query);

            var viewModel = new UsersViewModel()
            {
                Users = Mapper.Map<IEnumerable<ApplicationUserViewModel>>(users),
                PageNum = pageNum,
                SearchTerm = query,
                TotalPages = totalPages,
                Controller = "Admin",
                Action = "UsersSearch"
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UsersSearch(UsersViewModel viewModel)
        {
            return RedirectToAction("Users", new { pageNum = viewModel.PageNum, query = viewModel.SearchTerm });
        }

        public ActionResult ManageUser(string id = null)
        {
            if (string.IsNullOrEmpty(id))
                return View(new ManageUserViewModel());

            var user = _userService.GetUser(id);

            if (user == null)
                return HttpNotFound();

            var viewModel = new ManageUserViewModel()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Roles = Mapper.Map<IEnumerable<ApplicationRoleViewModel>>(user.Roles.Select(r => r.Role))
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(ManageUserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = _userService.CreateUser(viewModel.Name, viewModel.Email, viewModel.Email, viewModel.Roles1, _userManger);

                if (result.Succeeded)
                    return RedirectToAction("Users");

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

            return View("ManageUser", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateUser(ManageUserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = _userService.UpdateUser(viewModel.Id, viewModel.Name, viewModel.Email, viewModel.Roles1, _userManger);

                if (result.Succeeded)
                    return RedirectToAction("Users");

                ModelState.AddModelError("", result.ErrorMessage);
            }

            return View("ManageUser", viewModel);
        }
    }
}
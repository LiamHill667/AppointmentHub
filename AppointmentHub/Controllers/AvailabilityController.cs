using AppointmentHub.Core.Common;
using AppointmentHub.Core.Models;
using AppointmentHub.Core.Services;
using AppointmentHub.Core.ViewModels;
using AutoMapper;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AppointmentHub.Controllers
{
    [Authorize(Roles = AppRoles.Contact)]
    public class AvailabilityController : Controller
    {
        private readonly IUserAvailabilityService _userAvailabilityService;

        public AvailabilityController(IUserAvailabilityService userAvailabilityService)
        {
            _userAvailabilityService = userAvailabilityService;

        }

        // GET: Availability
        public ActionResult Index(string query = null, int pageNum = 0)
        {
            var availability = _userAvailabilityService
                .GetFutureUserAvailabilitiesPaged(User.Identity.GetUserId(), pageNum, out int totalPages, query);

            var viewModel = new AvailabilityViewModel()
            {
                Availability = Mapper.Map<IEnumerable<UserAvailabilityViewModel>>(availability),
                SearchTerm = query,
                PageNum = pageNum,
                TotalPages = totalPages,
                Controller = "Availability",
                Action = "Index"
            };

            return View("Availability", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(AvailabilityViewModel viewModel)
        {
            return RedirectToAction("Index", new { query = viewModel.SearchTerm, pageNum = viewModel.PageNum });

        }

        public ActionResult Create()
        {
            return View("AvailabilityForm", new AvailabilityFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AvailabilityFormViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                var userAvailability = Mapper.Map<UserAvailability>(viewModel);
                userAvailability.UserId = User.Identity.GetUserId();

                var result = _userAvailabilityService.CreateAvailability(userAvailability);

                if (result.Succeeded)
                    return RedirectToAction("Index");

                ModelState.AddModelError("User Availability", result.ErrorMessage);
            }

            return View("AvailabilityForm", viewModel);
        }

        public ActionResult Edit(int id)
        {
            var availability = _userAvailabilityService.GetAvailability(id);

            if (availability == null)
                return HttpNotFound();

            if (availability.UserId != User.Identity.GetUserId())
                return new HttpUnauthorizedResult();

            var viewModel = Mapper.Map<AvailabilityFormViewModel>(availability);

            return View("AvailabilityForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(AvailabilityFormViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var availability = Mapper.Map<UserAvailability>(viewModel);
                availability.UserId = User.Identity.GetUserId();

                var result = _userAvailabilityService.UpdateAvailability(availability);

                if (result.Succeeded)
                    return RedirectToAction("Index");

                ModelState.AddModelError("User Availability", result.ErrorMessage);
            }

            return View("AvailabilityForm", viewModel);
        }

    }
}
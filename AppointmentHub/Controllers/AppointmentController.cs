using AppointmentHub.Core.Services;
using AppointmentHub.Core.ViewModels;
using AutoMapper;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AppointmentHub.Controllers
{
    public class AppointmentController : Controller
    {

        private readonly IAppointmentService _appointmentService;
        private readonly IUserAvailabilityService _userAvailabilityService;

        public AppointmentController(IAppointmentService appointmentService, IUserAvailabilityService userAvailabilityService)
        {
            _appointmentService = appointmentService;
            _userAvailabilityService = userAvailabilityService;
        }

        // GET: Appointment
        [Authorize]
        public ActionResult Index(string query = null, int pageNum = 0)
        {
            var bookableAvailability = _userAvailabilityService
                .GetFutureBookableAvailabilitiesPaged(User.Identity.GetUserId(), pageNum, out int totalPages, query);

            var viewModel = new AvailabilityViewModel()
            {
                Availability = Mapper.Map<IEnumerable<UserAvailabilityViewModel>>(bookableAvailability),
                SearchTerm = query,
                PageNum = pageNum,
                TotalPages = totalPages,
                Controller = "Appointment",
                Action = "Index"
            };

            return View("BookableAppointments", viewModel);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SearchBookable(AvailabilityViewModel viewModel)
        {
            return RedirectToAction("Index", new { query = viewModel.SearchTerm, pageNum = viewModel.PageNum });
        }

        [Authorize]
        public ActionResult Mine(string query = null, int pageNum = 0)
        {
            var appointments = _appointmentService
                .GetAppointmentsPaged(User.Identity.GetUserId(), pageNum, out int totalPages, query);

            var viewModel = new AppointmentListViewModel()
            {
                Appointments = Mapper.Map<IEnumerable<AppointmentViewModel>>(appointments),
                SearchTerm = query,
                PageNum = pageNum,
                TotalPages = totalPages,
                Controller = "Appointment",
                Action = "Mine"
            };

            return View(viewModel);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SearchMine(AppointmentListViewModel viewModel)
        {
            return RedirectToAction("Mine", new { query = viewModel.SearchTerm, pageNum = viewModel.PageNum });
        }

        [Authorize]
        public ActionResult Book(int availabilityId)
        {
            var availability = _userAvailabilityService.GetAvailability(availabilityId);

            if (availability == null)
                return HttpNotFound();

            var types = _appointmentService.GetAppointmentTypes();

            var viewModel = new BookFormViewModel()
            {
                Availability = Mapper.Map<UserAvailabilityViewModel>(availability),
                Types = Mapper.Map<IEnumerable<AppointmentTypeViewModel>>(types),
                StartTime = availability.GetStartTime(),
                SelectableStartTimes = _appointmentService.GetAppointmentStartTimes(availability),
                SelectableEndTimes = _appointmentService.GetAppointmentEndTimes(availability)

            };

            return View("BookForm", viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Book(BookFormViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = _appointmentService.BookAppointment(viewModel.Subject, viewModel.DateTime,
                                                    viewModel.TimeSpan, User.Identity.GetUserId(), viewModel.Type.Id, viewModel.Availability.Id);

                if (result.Succeeded)
                    return RedirectToAction("Index");

                ModelState.AddModelError("Appointment", result.ErrorMessage);
            }

            var types = _appointmentService.GetAppointmentTypes();
            var availability = _userAvailabilityService.GetAvailability(viewModel.Availability.Id);
            viewModel.Types = Mapper.Map<IEnumerable<AppointmentTypeViewModel>>(types);
            viewModel.SelectableStartTimes = _appointmentService.GetAppointmentStartTimes(availability);
            viewModel.SelectableEndTimes = _appointmentService.GetAppointmentEndTimes(availability);

            return View("BookForm", viewModel);

        }


    }
}
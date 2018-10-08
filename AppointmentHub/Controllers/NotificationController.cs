using AppointmentHub.Core.Models;
using AppointmentHub.Core.Services;
using AppointmentHub.Core.ViewModels;
using AutoMapper;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;

namespace AppointmentHub.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // GET: Notification
        public ActionResult NotificationList()
        {
            var notifications = _notificationService.GetNewNotifications(User.Identity.GetUserId());
            var viewModel = notifications.Select(Mapper.Map<Notification, NotificationViewModel>);

            return PartialView("_NotificationListPartial", viewModel);
        }
    }
}
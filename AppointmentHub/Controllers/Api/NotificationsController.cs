using AppointmentHub.Core.Models;
using AppointmentHub.Core.Services;
using AppointmentHub.Core.ViewModels;
using AutoMapper;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace AppointmentHub.Controllers.Api
{
    [Authorize]
    public class NotificationsController : ApiController
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public IEnumerable<NotificationViewModel> GetNewNotifications()
        {
            return _notificationService.GetNewNotifications(User.Identity.GetUserId())
                .Select(Mapper.Map<Notification, NotificationViewModel>);
        }

        [HttpPost]
        public IHttpActionResult MarkAsRead()
        {
            var result = _notificationService.MarkAsRead(User.Identity.GetUserId());

            if (!result.Succeeded)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }
    }
}

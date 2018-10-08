using AppointmentHub.Core.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace AppointmentHub.Tests.Domain.Models
{
    [TestClass]
    public class ApplicationUserTests
    {
        [TestMethod]
        public void Notify_WhenCalled_ShouldAddTheNotification()
        {
            var user = new ApplicationUser();
            var notification = Notification.ApppointmentCanceled(new Appointment());

            user.Notify(notification);

            user.UserNotifications.Count.Should().Be(1);

            var userNotification = user.UserNotifications.First();
            userNotification.Notification.Should().Be(notification);
            userNotification.User.Should().Be(user);
        }

        [TestMethod]
        public void Notify_WhenCalledWithNull_NoNotificationAdded()
        {
            var user = new ApplicationUser();

            user.Notify(null);

            user.UserNotifications.Count.Should().Be(0);
        }
    }
}

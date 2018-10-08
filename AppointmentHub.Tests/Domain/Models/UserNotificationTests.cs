using AppointmentHub.Core.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AppointmentHub.Tests.Domain.Models
{
    [TestClass]
    public class UserNotificationTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "user")]
        public void UserNotification_NullUser_ArgumentNullException()
        {
            var userNotification = new UserNotification(
                null,
                Notification.AppointmentCreated(new Appointment()));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "notification")]
        public void UserNotification_NullNotification_ArgumentNullException()
        {
            var userNotification = new UserNotification(
                new ApplicationUser(),
                null);
        }

        [TestMethod]
        public void Read_WhenCalled_IsReadTrue()
        {
            var userNotification = new UserNotification(
                new ApplicationUser(),
                Notification.AppointmentCreated(new Appointment()));

            userNotification.Read();

            userNotification.IsRead.Should().Be(true);
        }
    }
}

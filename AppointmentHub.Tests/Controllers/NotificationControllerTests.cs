using AppointmentHub.Controllers;
using AppointmentHub.Core.Models;
using AppointmentHub.Core.Services;
using AppointmentHub.Core.ViewModels;
using AppointmentHub.Tests.Extensions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AppointmentHub.Tests.Controllers
{
    [TestClass]
    public class NotificationControllerTests
    {
        private NotificationController _controller;
        private Mock<INotificationService> _mockService;
        private string _userId;


        [TestInitialize]
        public void TestInitialize()
        {

            _mockService = new Mock<INotificationService>();

            _controller = new NotificationController(_mockService.Object);
            _userId = "1";
            _controller.MockCurrentUser(_userId);

        }

        #region NotificationList

        [TestMethod]
        public void NotificationList_NotificationsFromService_PartialViewWithNotificationViewModelReturned()
        {
            //Arrange
            IEnumerable<Notification> notifications = new Notification[] { };
            _mockService.Setup(r => r.GetNewNotifications(_userId))
                .Returns(notifications);

            //Act
            var result = _controller.NotificationList() as PartialViewResult;
            var model = result.Model as IEnumerable<NotificationViewModel>;

            //Assert
            result.Should().NotBeNull();
            result.ViewName.Should().Be("_NotificationListPartial");
            model.Should().HaveCount(0);
        }

        [TestMethod]
        public void NotificationList_NoNotificationsFromService_PartialViewWithNotificationViewModelReturned()
        {
            //Arrange
            var expectedAppointment = new Appointment()
            {
                Id = 123
            };
            var expectedNotification = Notification.AppointmentCreated(expectedAppointment);
            IEnumerable<Notification> notifications = new Notification[] { expectedNotification };
            _mockService.Setup(r => r.GetNewNotifications(_userId))
                .Returns(notifications);

            //Act
            var result = _controller.NotificationList() as PartialViewResult;
            var model = result.Model as IEnumerable<NotificationViewModel>;

            //Assert
            result.Should().NotBeNull();
            result.ViewName.Should().Be("_NotificationListPartial");
            model.Should().HaveCount(1);

            var notification = model.First();
            notification.Type = expectedNotification.Type;
            notification.Appointment.Id = expectedAppointment.Id;

        }

        #endregion
    }

}

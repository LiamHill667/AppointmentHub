using AppointmentHub.App_Start;
using AppointmentHub.Controllers.Api;
using AppointmentHub.Core.Models;
using AppointmentHub.Core.Services;
using AppointmentHub.Core.ViewModels;
using AppointmentHub.Tests.Extensions;
using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;

namespace AppointmentHub.Tests.Controllers.Api
{
    [TestClass]
    public class NotificationsControllerTests
    {
        private NotificationsController _controller;
        private Mock<INotificationService> _mockService;
        private string _userId;
        private string _userName;


        [TestInitialize]
        public void TestInitialize()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.AddProfile<MappingProfile>());

            _mockService = new Mock<INotificationService>();
            _controller = new NotificationsController(_mockService.Object);
            _userId = "1";
            _userName = "user1@domain.com";
            _controller.MockHttpContext(_userId, _userName);

        }

        [TestMethod]
        public void GetNewNotifications_ServiceReturnsNotifications_ReturnNotificationViewModelList()
        {
            var notifications = new[] { Notification.AppointmentCreated(new Appointment()) };

            _mockService
                .Setup(m => m.GetNewNotifications(_userId))
                .Returns(notifications);

            var results = _controller.GetNewNotifications();

            results.Should().AllBeOfType(typeof(NotificationViewModel));
            results.Count().Should().Be(1);
        }

        [TestMethod]
        public void GetNewNotifications_ServiceReturnsEmpty_ReturnNotificationViewModelList()
        {
            var notifications = new List<Notification>();

            _mockService
                .Setup(m => m.GetNewNotifications(_userId))
                .Returns(notifications);

            var results = _controller.GetNewNotifications();

            results.Should().AllBeOfType(typeof(NotificationViewModel));
            results.Should().BeEmpty();
        }

        [TestMethod]
        public void MarkAsRead_ServiceSucceeded_OkResult()
        {
            var serverResult = new ServiceResult(true);

            _mockService.Setup(m => m.MarkAsRead(_userId))
                .Returns(serverResult);

            var result = _controller.MarkAsRead();

            result.Should().BeOfType<OkResult>();
        }

        [TestMethod]
        public void MarkAsRead_ServiceFailuare_BadRequestResult()
        {
            var serverResult = new ServiceResult(false, "error message");

            _mockService.Setup(m => m.MarkAsRead(_userId))
                .Returns(serverResult);

            var result = _controller.MarkAsRead();

            result.Should().BeOfType<BadRequestErrorMessageResult>();
            ((BadRequestErrorMessageResult)result).Message.Should().Be(serverResult.ErrorMessage);

        }
    }
}

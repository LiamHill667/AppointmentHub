using AppointmentHub.Core;
using AppointmentHub.Core.Models;
using AppointmentHub.Core.Repositories;
using AppointmentHub.Core.Services;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;

namespace AppointmentHub.Tests.Services
{
    [TestClass]
    public class NotificationServiceTests
    {
        private NotificationService _service;
        private Mock<INotificationRepository> _mockNotificationRepository;
        private Mock<IUserNotificationRepository> _mockUserNotificationRepository;
        private Mock<IUnitOfWork> _mockUoW;
        private string _userId;


        [TestInitialize]
        public void TestInitialize()
        {
            _userId = "1";
            _mockNotificationRepository = new Mock<INotificationRepository>();
            _mockUserNotificationRepository = new Mock<IUserNotificationRepository>();

            _mockUoW = new Mock<IUnitOfWork>();
            _mockUoW.SetupGet(u => u.Notifications).Returns(_mockNotificationRepository.Object);
            _mockUoW.SetupGet(u => u.UserNotifications).Returns(_mockUserNotificationRepository.Object);

            _service = new NotificationService(_mockUoW.Object);

        }

        [TestMethod]
        public void GetNewNotifications_WhenCalled_ReturnNotificationViewModelList()
        {
            var notifications = new[] { Notification.AppointmentCreated(new Appointment()) };

            _mockNotificationRepository
                .Setup(m => m.GetNewNotificationsFor(_userId))
                .Returns(notifications);

            var results = _service.GetNewNotifications(_userId);

            results.Count().Should().Be(1);

            _mockNotificationRepository.Verify(m => m.GetNewNotificationsFor(_userId), Times.Once);
        }

        [TestMethod]
        public void MarkAsRead_WhenCalled_NotificationsRead()
        {
            var userNotifications = new[] { new UserNotification(new ApplicationUser(), Notification.AppointmentCreated(new Appointment())) };

            _mockUserNotificationRepository
                .Setup(m => m.GetUserNotificationsFor(_userId))
                .Returns(userNotifications);

            var result = _service.MarkAsRead(_userId);

            userNotifications.Any(n => n.IsRead == false).Should().Be(false);

            _mockUserNotificationRepository.Verify(m => m.GetUserNotificationsFor(_userId), Times.Once);
            _mockUoW.Verify(m => m.Complete(), Times.Once);

        }
    }
}

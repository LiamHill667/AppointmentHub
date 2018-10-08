using AppointmentHub.Core.Models;
using AppointmentHub.Persistence;
using AppointmentHub.Persistence.Repositories;
using AppointmentHub.Tests.Extensions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Data.Entity;
using System.Linq;

namespace AppointmentHub.Tests.Persistence.Repositories
{
    [TestClass]
    public class NotificationRepositoryTests
    {
        private NotificationRepository _repository;
        private Mock<DbSet<UserNotification>> _mockNotification;
        private Mock<IApplicationDbContext> _mockContext;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockNotification = new Mock<DbSet<UserNotification>>();
            _mockContext = new Mock<IApplicationDbContext>();

            _repository = new NotificationRepository(_mockContext.Object);
        }

        [TestMethod]
        public void GetNewNotificationsFor_ValidUserId_ShouldReturn()
        {
            var user = new ApplicationUser();
            var notification = Notification.AppointmentCreated(new Appointment());
            var userNotification = new UserNotification(user, notification);

            _mockNotification.SetSource(new[] { userNotification });

            _mockContext.SetupGet(c => c.UserNotifications).Returns(_mockNotification.Object);

            var result = _repository.GetNewNotificationsFor(user.Id);

            result.Should().Contain(notification);
            result.Count().Should().Be(1);

        }

        [TestMethod]
        public void GetNewNotificationsFor_InvalidUserId_ShouldReturn()
        {
            var user = new ApplicationUser();
            var notification = Notification.AppointmentCreated(new Appointment());
            var userNotification = new UserNotification(user, notification);

            _mockNotification.SetSource(new[] { userNotification });

            _mockContext.SetupGet(c => c.UserNotifications).Returns(_mockNotification.Object);

            var result = _repository.GetNewNotificationsFor(user.Id + "-");

            result.Should().BeEmpty();
        }

        [TestMethod]
        public void GetNewNotificationsFor_NotificationRead_ShouldNotReturn()
        {
            var user = new ApplicationUser();
            var notification = Notification.AppointmentCreated(new Appointment());
            var userNotification = new UserNotification(user, notification);
            userNotification.Read();

            _mockNotification.SetSource(new[] { userNotification });

            _mockContext.SetupGet(c => c.UserNotifications).Returns(_mockNotification.Object);

            var result = _repository.GetNewNotificationsFor(user.Id);

            result.Should().BeEmpty();
        }
    }
}

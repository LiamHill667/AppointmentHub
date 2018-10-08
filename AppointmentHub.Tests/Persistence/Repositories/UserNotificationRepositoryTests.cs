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
    public class UserNotificationRepositoryTests
    {
        private UserNotificationRepository _repository;
        private Mock<DbSet<UserNotification>> _mockUserNotification;
        private Mock<IApplicationDbContext> _mockContext;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockUserNotification = new Mock<DbSet<UserNotification>>();
            _mockContext = new Mock<IApplicationDbContext>();

            _repository = new UserNotificationRepository(_mockContext.Object);
        }

        [TestMethod]
        public void GetUserNotificationFor_ValidIdUnread_ShouldReturn()
        {
            var user = new ApplicationUser()
            {
                Id = "1"
            };

            var userNotification = new UserNotification(user, Notification.AppointmentCreated(new Appointment()));

            _mockUserNotification.SetSource(new[] { userNotification });

            _mockContext.SetupGet(c => c.UserNotifications).Returns(_mockUserNotification.Object);

            var result = _repository.GetUserNotificationsFor(user.Id);

            result.Should().Contain(userNotification);
            result.Count().Should().Be(1);
        }

        [TestMethod]
        public void GetUserNotificationFor_ValidIdRead_ShouldntReturn()
        {
            var user = new ApplicationUser()
            {
                Id = "1"
            };

            var userNotification = new UserNotification(user, Notification.AppointmentCreated(new Appointment()));
            userNotification.Read();

            _mockUserNotification.SetSource(new[] { userNotification });

            _mockContext.SetupGet(c => c.UserNotifications).Returns(_mockUserNotification.Object);

            var result = _repository.GetUserNotificationsFor(user.Id);

            result.Count().Should().Be(0);
        }

        [TestMethod]
        public void GetUserNotificationFor_InValidIdUnread_ShouldntReturn()
        {
            var user = new ApplicationUser()
            {
                Id = "1"
            };

            var userNotification = new UserNotification(user, Notification.AppointmentCreated(new Appointment()));

            _mockUserNotification.SetSource(new[] { userNotification });

            _mockContext.SetupGet(c => c.UserNotifications).Returns(_mockUserNotification.Object);

            var result = _repository.GetUserNotificationsFor(user.Id + "-");

            result.Count().Should().Be(0);
        }
    }
}

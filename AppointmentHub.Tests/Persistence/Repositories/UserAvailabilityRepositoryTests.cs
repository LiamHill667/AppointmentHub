using AppointmentHub.Core.Models;
using AppointmentHub.Persistence;
using AppointmentHub.Persistence.Repositories;
using AppointmentHub.Tests.Extensions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Data.Entity;
using System.Linq;

namespace AppointmentHub.Tests.Persistence.Repositories
{
    [TestClass]
    public class UserAvailabilityRepositoryTests
    {
        private UserAvailabilityRepository _repository;
        private Mock<DbSet<UserAvailability>> _mockUserAvailability;
        private Mock<IApplicationDbContext> _mockContext;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockUserAvailability = new Mock<DbSet<UserAvailability>>();
            _mockContext = new Mock<IApplicationDbContext>();

            _repository = new UserAvailabilityRepository(_mockContext.Object);
        }

        #region GetFutureUserAvailabilities

        [TestMethod]
        public void GetFutureUserAvailabilities_AvailabilityIsInThePast_ShouldNotBeReturned()
        {

            var availability = new UserAvailability()
            {
                DateTime = DateTime.Now.AddDays(-1),
                UserId = "1"
            };

            _mockUserAvailability.SetSource(new[] { availability });

            _mockContext.SetupGet(c => c.UserAvailabilties).Returns(_mockUserAvailability.Object);


            var userAvailabilties = _repository.GetFutureBookableUserAvailabilities(availability.UserId + "-");

            userAvailabilties.Should().BeEmpty();
        }

        [TestMethod]
        public void GetFutureUserAvailabilities_AvailabilityForDifferentUser_ShouldNotBeReturned()
        {
            var availability = new UserAvailability()
            {
                DateTime = DateTime.Now.AddDays(1),
                UserId = "1"
            };

            _mockUserAvailability.SetSource(new[] { availability });

            _mockContext.SetupGet(c => c.UserAvailabilties).Returns(_mockUserAvailability.Object);


            var userAvailabilties = _repository.GetFutureUserAvailabilities(availability.UserId + "-");

            userAvailabilties.Should().BeEmpty();
        }

        [TestMethod]
        public void GetFutureUserAvailabilities_AvailabilityForGivenUserAndIsInTheFuture_ShouldBeReturned()
        {
            var availability = new UserAvailability()
            {
                DateTime = DateTime.Now.AddDays(1),
                UserId = "1"
            };

            _mockUserAvailability.SetSource(new[] { availability });

            _mockContext.SetupGet(c => c.UserAvailabilties).Returns(_mockUserAvailability.Object);


            var userAvailabilties = _repository.GetFutureUserAvailabilities(availability.UserId);

            userAvailabilties.Should().Contain(availability);

        }

        #endregion

        #region GetFutureUserAvailabilitiesPaged

        [TestMethod]
        [ExpectedException(typeof(FormatException), "searchTerm string is not in parsable datetime format")]
        public void GetFutureUserAvailabilitiesPaged_InvalidSearchTerm_ShouldNotBeReturned()
        {
            string userId = "1";
            int pageNum = 0;
            string searchTerm = "test";

            var availability = new UserAvailability()
            {
                DateTime = DateTime.Now.AddDays(1),
                UserId = userId
            };

            _mockUserAvailability.SetSource(new[] { availability });

            _mockContext.SetupGet(c => c.UserAvailabilties).Returns(_mockUserAvailability.Object);

            var userAvailabilties = _repository.GetFutureUserAvailabilitiesPaged(userId, pageNum, out int totalPages, searchTerm);

        }

        [TestMethod]
        public void GetFutureUserAvailabilitiesPaged_MatchOnSearchTerm_ShouldBeReturned()
        {
            string userId = "1";
            int pageNum = 0;
            string searchTerm = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");

            var availability = new UserAvailability()
            {
                DateTime = DateTime.Now.AddDays(1),
                UserId = userId
            };

            _mockUserAvailability.SetSource(new[] { availability });

            _mockContext.SetupGet(c => c.UserAvailabilties).Returns(_mockUserAvailability.Object);

            var userAvailabilties = _repository.GetFutureUserAvailabilitiesPaged(userId, pageNum, out int totalPages, searchTerm);

            userAvailabilties.Count().Should().Be(1);
        }

        [TestMethod]
        public void GetFutureUserAvailabilitiesPaged_NoMatchOnSearchTerm_ShouldNotBeReturned()
        {
            string userId = "1";
            int pageNum = 0;
            string searchTerm = DateTime.Now.ToString("dd/MM/yyyy");

            var availability = new UserAvailability()
            {
                DateTime = DateTime.Now.AddDays(1),
                UserId = userId
            };

            _mockUserAvailability.SetSource(new[] { availability });

            _mockContext.SetupGet(c => c.UserAvailabilties).Returns(_mockUserAvailability.Object);

            var userAvailabilties = _repository.GetFutureUserAvailabilitiesPaged(userId, pageNum, out int totalPages, searchTerm);

            userAvailabilties.Should().BeEmpty();
        }


        [TestMethod]
        public void GetFutureUserAvailabilitiesPaged_AvailabilityIsInThePast_ShouldNotBeReturned()
        {
            string userId = "1";
            int pageNum = 0;

            var availability = new UserAvailability()
            {
                DateTime = DateTime.Now.AddDays(-1),
                UserId = userId
            };

            _mockUserAvailability.SetSource(new[] { availability });

            _mockContext.SetupGet(c => c.UserAvailabilties).Returns(_mockUserAvailability.Object);

            var userAvailabilties = _repository.GetFutureUserAvailabilitiesPaged(userId, pageNum, out int totalPages);

            userAvailabilties.Should().BeEmpty();
        }

        [TestMethod]
        public void GetFutureUserAvailabilitiesPaged_AvailabilityForDifferentUser_ShouldNotBeReturned()
        {
            string userId = "1";
            int pageNum = 0;

            var availability = new UserAvailability()
            {
                DateTime = DateTime.Now.AddDays(1),
                UserId = userId
            };

            _mockUserAvailability.SetSource(new[] { availability });

            _mockContext.SetupGet(c => c.UserAvailabilties).Returns(_mockUserAvailability.Object);


            var userAvailabilties = _repository.GetFutureUserAvailabilitiesPaged(userId + "-", pageNum, out int totalPages);

            userAvailabilties.Should().BeEmpty();
        }

        [TestMethod]
        public void GetFutureUserAvailabilitiesPaged_AvailabilityForGivenUserAndIsInTheFuture_ShouldBeReturned()
        {
            string userId = "1";
            int pageNum = 0;

            var availability = new UserAvailability()
            {
                DateTime = DateTime.Now.AddDays(1),
                UserId = userId
            };

            _mockUserAvailability.SetSource(new[] { availability });

            _mockContext.SetupGet(c => c.UserAvailabilties).Returns(_mockUserAvailability.Object);


            var userAvailabilties = _repository.GetFutureUserAvailabilitiesPaged(userId, pageNum, out int totalPages);

            userAvailabilties.Should().Contain(availability);

        }

        #endregion

        #region GetFutureBookableUserAvailabilities

        [TestMethod]
        public void GetFutureBookableUserAvailabilities_BookableAvailabilityIsInThePast_ShouldNotBeReturned()
        {
            string currentUserId = "1";

            var availability = new UserAvailability()
            {
                DateTime = DateTime.Now.AddDays(-1),
                UserId = "2"
            };

            _mockUserAvailability.SetSource(new[] { availability });

            _mockContext.SetupGet(c => c.UserAvailabilties).Returns(_mockUserAvailability.Object);

            var userAvailabilties = _repository.GetFutureBookableUserAvailabilities(currentUserId);

            userAvailabilties.Should().BeEmpty();
        }

        [TestMethod]
        public void GetFutureBookableUserAvailabilities_FutureAvailabilityForCurrentUser_ShouldNotBeReturned()
        {
            string currentUserId = "1";
            var availability = new UserAvailability()
            {
                DateTime = DateTime.Now.AddDays(1),
                UserId = currentUserId
            };

            _mockUserAvailability.SetSource(new[] { availability });

            _mockContext.SetupGet(c => c.UserAvailabilties).Returns(_mockUserAvailability.Object);


            var userAvailabilties = _repository.GetFutureBookableUserAvailabilities(currentUserId);

            userAvailabilties.Should().BeEmpty();
        }

        [TestMethod]
        public void GetFutureBookableUserAvailabilities_AvailabilityForOtherUserAndIsInTheFuture_ShouldBeReturned()
        {
            string currentUserId = "1";
            var availability = new UserAvailability()
            {
                DateTime = DateTime.Now.AddDays(1),
                UserId = "2"
            };

            _mockUserAvailability.SetSource(new[] { availability });

            _mockContext.SetupGet(c => c.UserAvailabilties).Returns(_mockUserAvailability.Object);


            var userAvailabilties = _repository.GetFutureBookableUserAvailabilities(currentUserId);

            userAvailabilties.Should().Contain(availability);
        }

        #endregion

        #region GetFutureBookableUserAvailabilitiesPaged

        [TestMethod]
        [ExpectedException(typeof(FormatException), "searchTerm string is not in parsable datetime format")]
        public void GetFutureBookableUserAvailabilitiesPaged_InvalidSearchTerm_ShouldNotBeReturned()
        {
            string currentUserId = "2";
            string userId = "1";
            int pageNum = 0;
            string searchTerm = "test";

            var availability = new UserAvailability()
            {
                DateTime = DateTime.Now.AddDays(1),
                UserId = userId
            };

            _mockUserAvailability.SetSource(new[] { availability });

            _mockContext.SetupGet(c => c.UserAvailabilties).Returns(_mockUserAvailability.Object);

            var userAvailabilties = _repository.GetFutureBookableUserAvailabilitiesPaged(currentUserId, pageNum, out int totalPages, searchTerm);

        }

        [TestMethod]
        public void GetFutureBookableUserAvailabilitiesPaged_MatchOnSearchTerm_ShouldBeReturned()
        {
            string currentUserId = "2";
            string userId = "1";
            int pageNum = 0;
            string searchTerm = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");

            var availability = new UserAvailability()
            {
                DateTime = DateTime.Now.AddDays(1),
                UserId = userId
            };

            _mockUserAvailability.SetSource(new[] { availability });

            _mockContext.SetupGet(c => c.UserAvailabilties).Returns(_mockUserAvailability.Object);

            var userAvailabilties = _repository.GetFutureBookableUserAvailabilitiesPaged(currentUserId, pageNum, out int totalPages, searchTerm);

            userAvailabilties.Count().Should().Be(1);
        }

        [TestMethod]
        public void GetFutureBookableUserAvailabilitiesPaged_NoMatchOnSearchTerm_ShouldNotBeReturned()
        {
            string currentUserId = "2";
            string userId = "1";
            int pageNum = 0;
            string searchTerm = DateTime.Now.ToString("dd/MM/yyyy");

            var availability = new UserAvailability()
            {
                DateTime = DateTime.Now.AddDays(1),
                UserId = userId
            };

            _mockUserAvailability.SetSource(new[] { availability });

            _mockContext.SetupGet(c => c.UserAvailabilties).Returns(_mockUserAvailability.Object);

            var userAvailabilties = _repository.GetFutureBookableUserAvailabilitiesPaged(currentUserId, pageNum, out int totalPages, searchTerm);

            userAvailabilties.Should().BeEmpty();
        }


        [TestMethod]
        public void GetFutureBookableUserAvailabilitiesPaged_AvailabilityIsInThePast_ShouldNotBeReturned()
        {
            string currentUserId = "2";
            string userId = "1";
            int pageNum = 0;

            var availability = new UserAvailability()
            {
                DateTime = DateTime.Now.AddDays(-1),
                UserId = userId
            };

            _mockUserAvailability.SetSource(new[] { availability });

            _mockContext.SetupGet(c => c.UserAvailabilties).Returns(_mockUserAvailability.Object);

            var userAvailabilties = _repository.GetFutureBookableUserAvailabilitiesPaged(currentUserId, pageNum, out int totalPages);

            userAvailabilties.Should().BeEmpty();
        }

        [TestMethod]
        public void GetFutureBookableUserAvailabilitiesPaged_AvailabilityForDifferentUser_ShouldBeReturned()
        {
            string currentUserId = "2";
            string userId = "1";
            int pageNum = 0;

            var availability = new UserAvailability()
            {
                DateTime = DateTime.Now.AddDays(1),
                UserId = userId
            };

            _mockUserAvailability.SetSource(new[] { availability });

            _mockContext.SetupGet(c => c.UserAvailabilties).Returns(_mockUserAvailability.Object);


            var userAvailabilties = _repository.GetFutureBookableUserAvailabilitiesPaged(currentUserId, pageNum, out int totalPages);
            userAvailabilties.Should().Contain(availability);

        }

        [TestMethod]
        public void GetFutureBookableUserAvailabilitiesPaged_AvailabilityForGivenUserAndIsInTheFuture_ShouldNotBeReturned()
        {
            string currentUserId = "2";
            int pageNum = 0;

            var availability = new UserAvailability()
            {
                DateTime = DateTime.Now.AddDays(1),
                UserId = currentUserId
            };

            _mockUserAvailability.SetSource(new[] { availability });

            _mockContext.SetupGet(c => c.UserAvailabilties).Returns(_mockUserAvailability.Object);


            var userAvailabilties = _repository.GetFutureBookableUserAvailabilitiesPaged(currentUserId, pageNum, out int totalPages);

            userAvailabilties.Should().BeEmpty();

        }

        #endregion

    }
}

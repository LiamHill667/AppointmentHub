using AppointmentHub.Core;
using AppointmentHub.Core.Models;
using AppointmentHub.Core.Repositories;
using AppointmentHub.Core.Services;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AppointmentHub.Tests.Services
{
    [TestClass]
    public class UserAvailabilityServiceTests
    {
        private UserAvailabilityService _service;
        private Mock<IUserAvailabilityRepository> _mockRepository;
        private Mock<IUnitOfWork> _mockUoW;
        private string _userId;


        [TestInitialize]
        public void TestInitialize()
        {
            _userId = "1";
            _mockRepository = new Mock<IUserAvailabilityRepository>();
            _mockUoW = new Mock<IUnitOfWork>();
            _mockUoW.SetupGet(u => u.UserAvailability).Returns(_mockRepository.Object);
            _service = new UserAvailabilityService(_mockUoW.Object);
        }

        #region CreateAvailability

        [TestMethod]
        public void CreateAvailability_ValidAvailability_ServiceResultSuceeded()
        {
            var userAvailability = new UserAvailability();

            var result = _service.CreateAvailability(userAvailability);

            result.Succeeded.Should().BeTrue();
        }

        #endregion

        #region Delete

        [TestMethod]
        public void Delete_NoAvailabilityWithGivenIdExists_ShouldReturnNotFound()
        {
            var expectedResult = new ServiceResult(false, "Availability not found.");

            var result = _service.DeleteAvailability(1, _userId);

            result.Succeeded.Should().Be(expectedResult.Succeeded);
            result.ErrorMessage.Should().Be(expectedResult.ErrorMessage);
        }

        [TestMethod]
        public void Delete_AvailabilityForDifferentuser_ShouldReturnOk()
        {
            var expectedResult = new ServiceResult(false, "User does not have permission.");
            var availability = new UserAvailability()
            {
                UserId = _userId + "-"
            };

            _mockRepository.Setup(r => r.GetById(1)).Returns(availability);

            var result = _service.DeleteAvailability(1, _userId);

            result.Succeeded.Should().Be(expectedResult.Succeeded);
            result.ErrorMessage.Should().Be(expectedResult.ErrorMessage);
        }

        [TestMethod]
        public void Delete_ValidRequest_ShouldReturnSucceeded()
        {
            var expectedResult = new ServiceResult(true);
            var availability = new UserAvailability()
            {
                UserId = _userId
            };
            _mockRepository.Setup(r => r.GetById(1)).Returns(availability);

            var result = _service.DeleteAvailability(1, _userId);

            result.Succeeded.Should().Be(expectedResult.Succeeded);
            result.ErrorMessage.Should().Be(expectedResult.ErrorMessage);
        }

        #endregion

        #region GetAvailability

        [TestMethod]
        public void GetAvailability_WhenCalled_ReturnsAvailability()
        {
            //Arrange
            var availability = new UserAvailability()
            {
                Id = 1
            };

            _mockRepository.Setup(m => m.GetById(availability.Id))
                .Returns(availability);

            //Act
            var result = _service.GetAvailability(availability.Id);

            //Assert
            result.Should().NotBeNull();
            result.Should().Be(availability);
        }

        #endregion

        #region GetFutureBookableAvailabilitiesPaged


        [TestMethod]
        public void GetFutureBookableAvailabilitiesPaged_WhenCalledNullQuery_ReturnsBookableAvailabilities()
        {
            //Arrange
            var availabilities = new UserAvailability[] { new UserAvailability() };
            int pageNum = 0;
            int totalPages = 3;
            string query = null;

            _mockRepository.Setup(m => m.GetFutureBookableUserAvailabilitiesPaged(_userId, pageNum, out totalPages, query, It.IsAny<int>()))
                .Returns(availabilities);

            var result = _service.GetFutureBookableAvailabilitiesPaged(_userId, pageNum, out totalPages, query);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(availabilities);
        }

        [TestMethod]
        public void GetFutureBookableAvailabilitiesPaged_WhenCalledValidQuery_ReturnsBookableAvailabilities()
        {
            //Arrange
            var availabilities = new UserAvailability[] { new UserAvailability() };
            int pageNum = 0;
            int totalPages = 3;
            string query = "16/10/2018";

            _mockRepository.Setup(m => m.GetFutureBookableUserAvailabilitiesPaged(_userId, pageNum, out totalPages, query, It.IsAny<int>()))
                .Returns(availabilities);

            var result = _service.GetFutureBookableAvailabilitiesPaged(_userId, pageNum, out totalPages, query);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(availabilities);
        }


        [TestMethod]
        public void GetFutureBookableAvailabilitiesPaged_WhenCalledInvalidQuery_ReturnsEmptyBookableAvailabilities()
        {
            //Arrange
            var availabilities = new UserAvailability[] { };
            int pageNum = 0;
            int totalPages = 3;
            string query = "test";

            _mockRepository.Setup(m => m.GetFutureBookableUserAvailabilitiesPaged(_userId, pageNum, out totalPages, query, It.IsAny<int>()))
                .Returns(availabilities);

            var result = _service.GetFutureBookableAvailabilitiesPaged(_userId, pageNum, out totalPages, query);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(availabilities);
        }

        #endregion

        #region GetFutureUserAvailabilitiesPaged


        [TestMethod]
        public void GetFutureUserAvailabilitiesPaged_WhenCalledNullQuery_ReturnsUserAvailabilities()
        {
            //Arrange
            var availabilities = new UserAvailability[] { new UserAvailability() };
            int pageNum = 0;
            int totalPages = 3;
            string query = null;

            _mockRepository.Setup(m => m.GetFutureUserAvailabilitiesPaged(_userId, pageNum, out totalPages, query, It.IsAny<int>()))
                .Returns(availabilities);

            var result = _service.GetFutureUserAvailabilitiesPaged(_userId, pageNum, out totalPages, query);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(availabilities);
        }

        [TestMethod]
        public void GetFutureUserAvailabilitiesPaged_WhenCalledInvalidQuery_ReturnsEmptyUserAvailabilities()
        {
            //Arrange
            var availabilities = new UserAvailability[] { };
            int pageNum = 0;
            int totalPages = 3;
            string query = "test";

            _mockRepository.Setup(m => m.GetFutureUserAvailabilitiesPaged(_userId, pageNum, out totalPages, query, It.IsAny<int>()))
                .Returns(availabilities);

            var result = _service.GetFutureUserAvailabilitiesPaged(_userId, pageNum, out totalPages, query);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(availabilities);
        }

        [TestMethod]
        public void GetFutureUserAvailabilitiesPaged_WhenCalledValidQuery_ReturnsUserAvailabilities()
        {
            //Arrange
            var availabilities = new UserAvailability[] { new UserAvailability() };
            int pageNum = 0;
            int totalPages = 3;
            string query = "16/10/2018";

            _mockRepository.Setup(m => m.GetFutureUserAvailabilitiesPaged(_userId, pageNum, out totalPages, query, It.IsAny<int>()))
                .Returns(availabilities);

            var result = _service.GetFutureUserAvailabilitiesPaged(_userId, pageNum, out totalPages, query);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(availabilities);
        }

        #endregion

        #region UpdateAvailability

        [TestMethod]
        public void UpdateAvailability_ValidAvailability_ServiceResultSuceeded()
        {
            var userAvailability = new UserAvailability();

            var result = _service.UpdateAvailability(userAvailability);

            result.Succeeded.Should().BeTrue();
        }

        #endregion
    }

}

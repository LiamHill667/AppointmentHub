using AppointmentHub.Controllers.Api;
using AppointmentHub.Core.Services;
using AppointmentHub.Tests.Extensions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web.Http.Results;

namespace AppointmentHub.Tests.Controllers.Api
{
    [TestClass]
    public class AvailabilityControllerTests
    {
        private AvailabilityController _controller;
        private Mock<IUserAvailabilityService> _mockService;
        private string _userId;
        private string _userName;


        [TestInitialize]
        public void TestInitialize()
        {
            _userId = "1";
            _userName = "user1@domain.com";

            _mockService = new Mock<IUserAvailabilityService>();

            _controller = new AvailabilityController(_mockService.Object);
            _controller.MockHttpContext(_userId, _userName);

        }

        [TestMethod]
        public void Delete_NoAvailabilityWithGivenIdExists_BadRequest()
        {
            var serverResult = new ServiceResult(false, "error message");
            _mockService.Setup(m => m.DeleteAvailability(1, _userId))
                .Returns(serverResult);

            var result = _controller.Delete(1);

            result.Should().BeOfType<BadRequestErrorMessageResult>();
            ((BadRequestErrorMessageResult)result).Message.Should().Be(serverResult.ErrorMessage);
        }

        [TestMethod]
        public void Delete_ServiceSucceeded_OkResultWithId()
        {
            var serverResult = new ServiceResult(true);
            _mockService.Setup(m => m.DeleteAvailability(1, _userId))
                .Returns(serverResult);

            var result = (OkNegotiatedContentResult<int>)_controller.Delete(1);

            result.Content.Should().Be(1);
        }
    }
}

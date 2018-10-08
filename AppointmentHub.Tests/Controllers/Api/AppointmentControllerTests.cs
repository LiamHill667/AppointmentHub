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
    public class AppointmentControllerTests
    {
        private AppointmentController _controller;
        private Mock<IAppointmentService> _mockService;
        private string _userId;
        private string _userName;


        [TestInitialize]
        public void TestInitialize()
        {
            _userId = "1";
            _userName = "user1@domain.com";

            _mockService = new Mock<IAppointmentService>();
            _controller = new AppointmentController(_mockService.Object);
            _controller.MockHttpContext(_userId, _userName);

        }

        [TestMethod]
        public void Cancel_ServiceFailuare_Badrequest()
        {
            var expectedResult = new ServiceResult(false, "ErrorMessage");

            _mockService.Setup(m => m.CancelAppointment(1, _userId))
                .Returns(expectedResult);

            var result = _controller.Cancel(1);

            result.Should().BeOfType<BadRequestErrorMessageResult>();
            ((BadRequestErrorMessageResult)result).Message.Should().Be(expectedResult.ErrorMessage);
        }

        [TestMethod]
        public void Cancel_ServiceSucceeded_ShouldReturnOk()
        {
            var expectedResult = new ServiceResult(true);

            _mockService.Setup(m => m.CancelAppointment(1, _userId))
                .Returns(expectedResult);

            var result = _controller.Cancel(1);

            result.Should().BeOfType<OkResult>();
        }
    }
}

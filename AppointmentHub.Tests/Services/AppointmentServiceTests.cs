using AppointmentHub.Core;
using AppointmentHub.Core.Models;
using AppointmentHub.Core.Repositories;
using AppointmentHub.Core.Services;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;

namespace AppointmentHub.Tests.Services
{
    [TestClass]
    public class AppointmentServiceTests
    {
        private AppointmentService _service;
        private Mock<IAppointmentRepository> _mockRepository;
        private Mock<IAppointmentTypeRepository> _mockAppointmentTypeRepository;
        private Mock<IUserAvailabilityRepository> _mockUserAvailabilityRepository;
        private Mock<IUnitOfWork> _mockUoW;
        private string _userId;


        [TestInitialize]
        public void TestInitialize()
        {
            _userId = "1";
            _mockRepository = new Mock<IAppointmentRepository>();
            _mockAppointmentTypeRepository = new Mock<IAppointmentTypeRepository>();
            _mockUserAvailabilityRepository = new Mock<IUserAvailabilityRepository>();
            _mockUoW = new Mock<IUnitOfWork>();
            _mockUoW.SetupGet(u => u.Appointment).Returns(_mockRepository.Object);
            _mockUoW.SetupGet(u => u.AppointmentType).Returns(_mockAppointmentTypeRepository.Object);
            _mockUoW.SetupGet(u => u.UserAvailability).Returns(_mockUserAvailabilityRepository.Object);
            _service = new AppointmentService(_mockUoW.Object);
        }

        #region GetAppointmentsPaged

        [TestMethod]
        public void GetAppointmentsPaged_WhenCalledInvalidQuery_ReturnsEmptyAppointments()
        {
            //Arrange
            var appointments = new Appointment[] { };
            int pageNum = 0;
            int totalPages = 3;
            string query = "test";

            _mockRepository.Setup(m => m.GetMyAppointmentsPaged(_userId, pageNum, out totalPages, query, It.IsAny<int>()))
                .Returns(appointments);

            var result = _service.GetAppointmentsPaged(_userId, pageNum, out totalPages, query);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(appointments);
        }

        [TestMethod]
        public void GetAppointmentsPaged_WhenCalledNullQuery_ReturnsAppointments()
        {
            //Arrange
            var appointments = new Appointment[] { new Appointment() };
            int pageNum = 0;
            int totalPages = 3;
            string query = null;

            _mockRepository.Setup(m => m.GetMyAppointmentsPaged(_userId, pageNum, out totalPages, query, It.IsAny<int>()))
                .Returns(appointments);

            var result = _service.GetAppointmentsPaged(_userId, pageNum, out totalPages, query);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(appointments);
        }

        [TestMethod]
        public void GetAppointmentsPaged_WhenCalledValidQuery_ReturnsAppointments()
        {
            //Arrange
            var appointments = new Appointment[] { new Appointment() };
            int pageNum = 0;
            int totalPages = 3;
            string query = "16/10/2018";

            _mockRepository.Setup(m => m.GetMyAppointmentsPaged(_userId, pageNum, out totalPages, query, It.IsAny<int>()))
                .Returns(appointments);

            var result = _service.GetAppointmentsPaged(_userId, pageNum, out totalPages, query);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(appointments);
        }

        #endregion

        #region GetAppointmentStartTimes


        [TestMethod]
        public void GetAppointmentStartTimes_AvailabilityStartTime0000_ReturnsTimeSpansGreaterThanavailabiltyStartTime()
        {
            var availability = new UserAvailability()
            {
                DateTime = new DateTime(2018, 10, 5, 0, 0, 0)
            };

            var result = _service.GetAppointmentStartTimes(availability);

            result.Any(t => t < availability.GetStartTime()).Should().BeFalse();
            result.Should().HaveCount(24);
        }

        #endregion

        #region GetApppointmentEndTimes


        [TestMethod]
        public void GetAppointmentEndTimes_AvailabilityStartTime0000EndTime04000_ReturnsTimeSpansGreaterThanavailabiltyStartTimeLessThanEndTime()
        {
            var availability = new UserAvailability()
            {
                DateTime = new DateTime(2018, 10, 5, 0, 0, 0),
                TimeSpan = new TimeSpan(4, 0, 0)
            };

            var result = _service.GetAppointmentEndTimes(availability);

            result.All(t => t > availability.GetStartTime()).Should().BeTrue();
            result.All(t => t <= availability.GetEndTime()).Should().BeTrue();
            result.Should().HaveCount(4);
        }

        #endregion

        #region GetAppointmentTypes

        [TestMethod]
        public void GetAppointmentTypes_WhenCalled_ReturnsAppointmentTypes()
        {
            //Arrange
            var appointmentTypes = new AppointmentType[] { new AppointmentType() };

            _mockAppointmentTypeRepository.Setup(m => m.GetAll())
                .Returns(appointmentTypes);

            //Act
            var result = _service.GetAppointmentTypes();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(appointmentTypes);
        }

        #endregion

        #region BookAppointment
        /**
         * Need 6 tests
         * 1 availability not found so returned service fail
         * 2 valid returns service success
         * 3 availability and appointment have same datetime and time span. delete availability add appointment
         * 4 availability and apointment only start at the same time, availbility start time is set to the end of the appointment and time span set approiately
         * 5 availability and appointment have the same end time, reduce the length on the availability to match the start of the appointment
         * 6 availability is slipt by the appointment. availability time span matches the start of the appointment. new availability created that starts at the end of
         * the appointment and 
         * **/

        [TestMethod]
        public void BookAppointment_AvailabilityNotFound_ReturnServiceFailuare()
        {
            var expectedResult = new ServiceResult(false, "User Availability not Found");
            UserAvailability availability = null;
            int availabilityId = 1;
            _mockUserAvailabilityRepository.Setup(m => m.GetById(availabilityId)).Returns(availability);

            var result = _service.BookAppointment("test", DateTime.Now, TimeSpan.Zero, "", 0, availabilityId);

            result.Should().NotBeNull();
            result.Succeeded.Should().Be(expectedResult.Succeeded);
            result.ErrorMessage.Should().Be(expectedResult.ErrorMessage);

        }

        [TestMethod]
        public void BookAppointment_Success_ReturnServiceSuccess()
        {
            var expectedResult = new ServiceResult(true);
            UserAvailability availability = new UserAvailability()
            {
                Id = 1,
            };
            _mockUserAvailabilityRepository.Setup(m => m.GetById(availability.Id)).Returns(availability);

            var result = _service.BookAppointment("test", DateTime.Now, TimeSpan.Zero, "", 0, availability.Id);

            result.Should().NotBeNull();
            result.Succeeded.Should().Be(expectedResult.Succeeded);
            result.ErrorMessage.Should().Be(expectedResult.ErrorMessage);

        }
        #endregion

        #region Cancel

        [TestMethod]
        public void Cancel_AppointmentNotFound_ShouldReturnNotFound()
        {
            var expectedResult = new ServiceResult(false, "Appointment cannot be found appointmentId: 1 or not authorized to access appointment");
            Appointment appointment = null;

            _mockRepository.Setup(r => r.GetById(1)).Returns(appointment);

            var result = _service.CancelAppointment(1, _userId);

            result.Succeeded.Should().Be(expectedResult.Succeeded);
            result.ErrorMessage.Should().Be(expectedResult.ErrorMessage);
        }


        [TestMethod]
        public void Cancel_AppointmentIsCanceled_Suceeded()
        {
            var expectedResult = new ServiceResult(true);
            var appointment = new Appointment
            {
                Requested = new ApplicationUser(),
                Requestee = new ApplicationUser(),
                RequesteeId = _userId
            };

            appointment.Cancel();

            _mockRepository.Setup(r => r.GetById(1)).Returns(appointment);

            var result = _service.CancelAppointment(1, _userId);

            result.Succeeded.Should().Be(expectedResult.Succeeded);
            result.ErrorMessage.Should().Be(expectedResult.ErrorMessage);
        }

        [TestMethod]
        public void Cancel_UserCancelingAnotherUsersAppointment_ShouldReturnUnauthorized()
        {
            var expectedResult = new ServiceResult(false, "Appointment cannot be found appointmentId: 1 or not authorized to access appointment");
            var appointment = new Appointment
            {
                Requested = new ApplicationUser(),
                Requestee = new ApplicationUser(),
                RequesteeId = _userId + "-"
            };

            _mockRepository.Setup(r => r.GetById(1)).Returns(appointment);

            var result = _service.CancelAppointment(1, _userId);

            result.Succeeded.Should().Be(expectedResult.Succeeded);
            result.ErrorMessage.Should().Be(expectedResult.ErrorMessage);
        }

        [TestMethod]
        public void Cancel_ValidRequest_ShouldReturnOk()
        {
            var expectedResult = new ServiceResult(true);
            var appointment = new Appointment
            {
                Requested = new ApplicationUser(),
                Requestee = new ApplicationUser(),
                RequesteeId = _userId
            };

            _mockRepository.Setup(r => r.GetById(1)).Returns(appointment);

            var result = _service.CancelAppointment(1, _userId);

            result.Succeeded.Should().Be(expectedResult.Succeeded);
            result.ErrorMessage.Should().Be(expectedResult.ErrorMessage);
        }

        #endregion
    }
}

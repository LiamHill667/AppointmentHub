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
    public class AppointmentRepositoryTests
    {
        private AppointmentRepository _repository;
        private Mock<DbSet<Appointment>> _mockAppointment;
        private Mock<IApplicationDbContext> _mockContext;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockAppointment = new Mock<DbSet<Appointment>>();
            _mockContext = new Mock<IApplicationDbContext>();

            _repository = new AppointmentRepository(_mockContext.Object);
        }

        #region GetById

        [TestMethod]
        public void GetById_WhenCalledValidId_ShouldReturn()
        {
            var appointment = new Appointment()
            {
                Id = 1
            };

            _mockAppointment.SetSource(new[] { appointment });

            _mockContext.SetupGet(c => c.Appointments).Returns(_mockAppointment.Object);

            var result = _repository.GetById(appointment.Id);

            result.Should().Be(appointment);
        }

        [TestMethod]
        public void GetById_WhenCalledInvalidId_ShouldReturn()
        {
            var appointment = new Appointment()
            {
                Id = 1
            };

            _mockAppointment.SetSource(new[] { appointment });

            _mockContext.SetupGet(c => c.Appointments).Returns(_mockAppointment.Object);

            var result = _repository.GetById(appointment.Id + 1);

            result.Should().Be(null);
        }

        #endregion

        #region GetMyAppointmenets

        [TestMethod]
        public void GetMyAppointments_ValidAppointmentUserId_ShouldReturn()
        {
            string userId = "1";
            var appointment = new Appointment()
            {
                RequesteeId = userId,
                IsCanceled = false
            };

            _mockAppointment.SetSource(new[] { appointment });

            _mockContext.SetupGet(c => c.Appointments).Returns(_mockAppointment.Object);

            var result = _repository.GetMyAppointments(userId);

            result.Count().Should().Be(1);
            result.Should().Contain(appointment);
        }

        [TestMethod]
        public void GetMyAppointments_InvalidAppointmentUserId_ShouldNotReturn()
        {
            string userId = "1";
            var appointment = new Appointment()
            {
                RequesteeId = userId,
                IsCanceled = false
            };

            _mockAppointment.SetSource(new[] { appointment });

            _mockContext.SetupGet(c => c.Appointments).Returns(_mockAppointment.Object);

            var result = _repository.GetMyAppointments(userId + "-");

            result.Count().Should().Be(0);
        }

        [TestMethod]
        public void GetMyAppointments_AppointmentCanceled_ShouldNotReturn()
        {
            string userId = "1";
            var appointment = new Appointment()
            {
                RequesteeId = userId,
                IsCanceled = true
            };

            _mockAppointment.SetSource(new[] { appointment });

            _mockContext.SetupGet(c => c.Appointments).Returns(_mockAppointment.Object);

            var result = _repository.GetMyAppointments(userId);

            result.Count().Should().Be(0);
        }

        #endregion

        #region GetMyAppointmentsPaged

        [TestMethod]
        public void GetMyAppointmentsPaged_ValidAppointmentUserId_ShouldReturn()
        {
            string userId = "1";
            int pageNum = 0;
            var appointment = new Appointment()
            {
                RequesteeId = userId,
                IsCanceled = false
            };

            _mockAppointment.SetSource(new[] { appointment });

            _mockContext.SetupGet(c => c.Appointments).Returns(_mockAppointment.Object);

            var result = _repository.GetMyAppointmentsPaged(userId, pageNum, out int totalPages);

            result.Count().Should().Be(1);
            result.Should().Contain(appointment);
        }

        [TestMethod]
        public void GetMyAppointmentsPaged_InvalidAppointmentUserId_ShouldNotReturn()
        {
            string userId = "1";
            int pageNum = 0;
            var appointment = new Appointment()
            {
                RequesteeId = userId,
                IsCanceled = false
            };

            _mockAppointment.SetSource(new[] { appointment });

            _mockContext.SetupGet(c => c.Appointments).Returns(_mockAppointment.Object);

            var result = _repository.GetMyAppointmentsPaged(userId + "-", pageNum, out int totalPages);

            result.Count().Should().Be(0);
        }

        [TestMethod]
        public void GetMyAppointmentsPaged_AppointmentCanceled_ShouldNotReturn()
        {
            string userId = "1";
            int pageNum = 0;
            var appointment = new Appointment()
            {
                RequesteeId = userId,
                IsCanceled = true
            };

            _mockAppointment.SetSource(new[] { appointment });

            _mockContext.SetupGet(c => c.Appointments).Returns(_mockAppointment.Object);

            var result = _repository.GetMyAppointmentsPaged(userId, pageNum, out int totalPages);

            result.Count().Should().Be(0);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException), "searchTerm string is not in parsable datetime format")]
        public void GetMyAppointmentsPaged_SearchTermInvalid_ShouldNotReturn()
        {
            string userId = "1";
            int pageNum = 0;
            string SearchTerm = "test";
            var appointment = new Appointment()
            {
                RequesteeId = userId,
                IsCanceled = false,
                DateTime = DateTime.Now
            };

            _mockAppointment.SetSource(new[] { appointment });

            _mockContext.SetupGet(c => c.Appointments).Returns(_mockAppointment.Object);

            var result = _repository.GetMyAppointmentsPaged(userId, pageNum, out int totalPages, SearchTerm);
        }

        [TestMethod]
        public void GetMyAppointmentsPaged_SearchTermValidMatch_ShouldReturn()
        {
            string userId = "1";
            int pageNum = 0;
            string SearchTerm = DateTime.Now.ToString("dd/MM/yyyy");
            var appointment = new Appointment()
            {
                RequesteeId = userId,
                IsCanceled = false,
                DateTime = DateTime.Now
            };

            _mockAppointment.SetSource(new[] { appointment });

            _mockContext.SetupGet(c => c.Appointments).Returns(_mockAppointment.Object);

            var result = _repository.GetMyAppointmentsPaged(userId, pageNum, out int totalPages, SearchTerm);

            result.Count().Should().Be(1);
            result.Should().Contain(appointment);
        }

        [TestMethod]
        public void GetMyAppointmentsPaged_SearchTermNoMatch_ShouldNotReturn()
        {
            string userId = "1";
            int pageNum = 0;
            string SearchTerm = DateTime.Now.ToString("dd/MM/yyyy");
            var appointment = new Appointment()
            {
                RequesteeId = userId,
                IsCanceled = false,
                DateTime = DateTime.Now.AddDays(1)
            };

            _mockAppointment.SetSource(new[] { appointment });

            _mockContext.SetupGet(c => c.Appointments).Returns(_mockAppointment.Object);

            var result = _repository.GetMyAppointmentsPaged(userId, pageNum, out int totalPages, SearchTerm);

            result.Count().Should().Be(0);
        }
        #endregion
    }
}

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
    public class GenericRepositoryTests
    {
        private GenericRepository<Appointment> _repository;
        private Mock<DbSet<Appointment>> _mockAppointment;
        private Mock<IApplicationDbContext> _mockContext;

        [TestInitialize]
        public void TestInitialize()
        {

        }

        [TestMethod]
        public void GetById_WhenCalledValidId_ShouldReturn()
        {
            _mockAppointment = new Mock<DbSet<Appointment>>();
            var appointment = new Appointment()
            {
                Id = 1
            };

            _mockAppointment.SetSource(new[] { appointment });
            _mockAppointment.Setup(a => a.Find(appointment.Id)).Returns(appointment);

            _mockContext = new Mock<IApplicationDbContext>();
            _mockContext.Setup(c => c.Set<Appointment>()).Returns(_mockAppointment.Object);

            _repository = new AppointmentRepository(_mockContext.Object);


            var result = _repository.GetById(appointment.Id);

            result.Should().Be(appointment);
            _mockAppointment.Verify(c => c.Find(appointment.Id), Times.Once);
        }

        [TestMethod]
        public void Add_WhenCalledValidEntity_ShouldCallAdd()
        {
            _mockAppointment = new Mock<DbSet<Appointment>>();
            var appointment = new Appointment()
            {
                Id = 1
            };

            _mockAppointment.Setup(a => a.Add(appointment)).Returns(appointment);

            _mockContext = new Mock<IApplicationDbContext>();
            _mockContext.Setup(c => c.Set<Appointment>()).Returns(_mockAppointment.Object);

            _repository = new AppointmentRepository(_mockContext.Object);


            _repository.Add(appointment);

            _mockAppointment.Verify(c => c.Add(appointment), Times.Once);
        }

        [TestMethod]
        public void Delete_CalledWithEntity_RemoveCalled()
        {
            _mockAppointment = new Mock<DbSet<Appointment>>();
            var appointment = new Appointment()
            {
                Id = 1
            };

            _mockAppointment.SetSource(new[] { appointment });
            _mockAppointment.Setup(a => a.Find(appointment.Id)).Returns(appointment);
            _mockAppointment.Setup(a => a.Remove(appointment)).Returns(appointment);

            _mockContext = new Mock<IApplicationDbContext>();
            _mockContext.Setup(c => c.GetEntityState(appointment)).Returns(EntityState.Unchanged);
            _mockContext.Setup(c => c.Set<Appointment>()).Returns(_mockAppointment.Object);

            _repository = new AppointmentRepository(_mockContext.Object);


            _repository.Delete(appointment);

            _mockContext.Verify(c => c.GetEntityState(appointment), Times.Once);
            _mockAppointment.Verify(c => c.Remove(appointment), Times.Once);
        }

        [TestMethod]
        public void Delete_CalledWithDetachedEntity_RemoveCalledAfterAttach()
        {
            _mockAppointment = new Mock<DbSet<Appointment>>();
            var appointment = new Appointment()
            {
                Id = 1
            };

            _mockAppointment.SetSource(new[] { appointment });
            _mockAppointment.Setup(a => a.Find(appointment.Id)).Returns(appointment);
            _mockAppointment.Setup(a => a.Attach(appointment)).Returns(appointment);
            _mockAppointment.Setup(a => a.Remove(appointment)).Returns(appointment);
            _mockContext = new Mock<IApplicationDbContext>();
            _mockContext.Setup(c => c.GetEntityState(appointment)).Returns(EntityState.Detached);
            _mockContext.Setup(c => c.Set<Appointment>()).Returns(_mockAppointment.Object);

            _repository = new AppointmentRepository(_mockContext.Object);


            _repository.Delete(appointment);

            _mockContext.Verify(c => c.GetEntityState(appointment), Times.Once);
            _mockAppointment.Verify(c => c.Attach(appointment), Times.Once);
            _mockAppointment.Verify(c => c.Remove(appointment), Times.Once);
        }

        [TestMethod]
        public void Delete_CalledWithIdAttached_RemoveCalled()
        {
            _mockAppointment = new Mock<DbSet<Appointment>>();
            var appointment = new Appointment()
            {
                Id = 1
            };

            _mockAppointment.SetSource(new[] { appointment });
            _mockAppointment.Setup(a => a.Find(appointment.Id)).Returns(appointment);
            _mockAppointment.Setup(a => a.Attach(appointment)).Returns(appointment);
            _mockAppointment.Setup(a => a.Remove(appointment)).Returns(appointment);
            _mockContext = new Mock<IApplicationDbContext>();
            _mockContext.Setup(c => c.GetEntityState(appointment)).Returns(EntityState.Unchanged);
            _mockContext.Setup(c => c.Set<Appointment>()).Returns(_mockAppointment.Object);

            _repository = new AppointmentRepository(_mockContext.Object);


            _repository.Delete(appointment.Id);

            _mockContext.Verify(c => c.GetEntityState(appointment), Times.Once);
            _mockAppointment.Verify(c => c.Attach(appointment), Times.Never);
            _mockAppointment.Verify(c => c.Remove(appointment), Times.Once);
        }

        [TestMethod]
        public void Delete_CalledWithIdDetached_RemoveCalledAfterAttach()
        {
            _mockAppointment = new Mock<DbSet<Appointment>>();
            var appointment = new Appointment()
            {
                Id = 1
            };

            _mockAppointment.SetSource(new[] { appointment });
            _mockAppointment.Setup(a => a.Find(appointment.Id)).Returns(appointment);
            _mockAppointment.Setup(a => a.Attach(appointment)).Returns(appointment);
            _mockAppointment.Setup(a => a.Remove(appointment)).Returns(appointment);
            _mockContext = new Mock<IApplicationDbContext>();
            _mockContext.Setup(c => c.GetEntityState(appointment)).Returns(EntityState.Detached);
            _mockContext.Setup(c => c.Set<Appointment>()).Returns(_mockAppointment.Object);

            _repository = new AppointmentRepository(_mockContext.Object);


            _repository.Delete(appointment.Id);

            _mockContext.Verify(c => c.GetEntityState(appointment), Times.Once);
            _mockAppointment.Verify(c => c.Attach(appointment), Times.Once);
            _mockAppointment.Verify(c => c.Remove(appointment), Times.Once);
        }


        [TestMethod]
        public void Update_CalledWithEntity_RemoveCalledAfterAttach()
        {
            _mockAppointment = new Mock<DbSet<Appointment>>();
            var appointment = new Appointment()
            {
                Id = 1
            };

            _mockAppointment.SetSource(new[] { appointment });
            _mockAppointment.Setup(a => a.Attach(appointment)).Returns(appointment);
            _mockContext = new Mock<IApplicationDbContext>();
            _mockContext.Setup(c => c.SetEntityModified(appointment));
            _mockContext.Setup(c => c.Set<Appointment>()).Returns(_mockAppointment.Object);

            _repository = new AppointmentRepository(_mockContext.Object);


            _repository.Update(appointment);

            _mockAppointment.Verify(c => c.Attach(appointment), Times.Once);
            _mockContext.Verify(c => c.SetEntityModified(appointment), Times.Once);
        }

        [TestMethod]
        public void GetAll_WhenCalled_ShouldReturn()
        {
            _mockAppointment = new Mock<DbSet<Appointment>>();
            var appointment = new Appointment()
            {
                Id = 1
            };

            _mockAppointment.SetSource(new[] { appointment });

            _mockContext = new Mock<IApplicationDbContext>();
            _mockContext.Setup(c => c.Set<Appointment>()).Returns(_mockAppointment.Object);

            _repository = new AppointmentRepository(_mockContext.Object);


            var result = _repository.GetAll();

            result.Count().Should().Be(1);
            result.Should().Contain(appointment);
        }
    }
}

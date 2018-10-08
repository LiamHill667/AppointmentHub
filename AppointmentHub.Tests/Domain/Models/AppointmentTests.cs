using AppointmentHub.Core.Common;
using AppointmentHub.Core.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace AppointmentHub.Tests.Domain.Models
{
    [TestClass]
    public class AppointmentTests
    {

        [TestMethod]
        public void Cancel_WhenCalled_AppointmentIsCanceledTrue()
        {
            var requestee = new ApplicationUser();
            var requested = new ApplicationUser();

            var appointment = new Appointment()
            {
                Requestee = requestee,
                Requested = requested
            };

            appointment.Cancel();

            appointment.IsCanceled.Should().Be(true);
        }

        [TestMethod]
        public void Cancel_WhenCalled_RequestedRequesteeNotified()
        {
            var requestee = new ApplicationUser();
            var requested = new ApplicationUser();

            var appointment = new Appointment()
            {
                Requestee = requestee,
                Requested = requested
            };

            appointment.Cancel();

            requestee.UserNotifications.Count.Should().Be(1);
            var requesteeNotification = requestee.UserNotifications.First();

            requested.UserNotifications.Count.Should().Be(1);
            var requestedNotification = requested.UserNotifications.First();

            requesteeNotification.Notification.Should().Be(requestedNotification.Notification);
            requesteeNotification.Notification.Type.Should().Be(NotificationType.AppointmentCanceled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Requested")]
        public void Cancel_WhenCalledWithInvalidAppointment_NullExceptionError()
        {
            var appointment = new Appointment();

            appointment.Cancel();

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Requestee")]
        public void Cancel_WhenCalledWithNullRequested_NullExceptionError()
        {
            var appointment = new Appointment()
            {
                Requested = new ApplicationUser()
            };

            appointment.Cancel();

        }

        [TestMethod]
        public void Booked_WhenCalled_RequestedRequesteeNotified()
        {
            var requestee = new ApplicationUser();
            var requested = new ApplicationUser();

            var appointment = new Appointment()
            {
                Requestee = requestee,
                Requested = requested
            };

            appointment.Booked();

            requestee.UserNotifications.Count.Should().Be(1);
            var requesteeNotification = requestee.UserNotifications.First();

            requested.UserNotifications.Count.Should().Be(1);
            var requestedNotification = requested.UserNotifications.First();

            requesteeNotification.Notification.Should().Be(requestedNotification.Notification);
            requesteeNotification.Notification.Type.Should().Be(NotificationType.AppointmentCreated);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Requested")]
        public void Booked_WhenCalledWithInvalidAppointment_NullExceptionError()
        {
            var appointment = new Appointment();

            appointment.Booked();

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Requestee")]
        public void Booked_WhenCalledWithNullRequested_NullExceptionError()
        {
            var appointment = new Appointment()
            {
                Requested = new ApplicationUser()
            };

            appointment.Booked();

        }
    }
}

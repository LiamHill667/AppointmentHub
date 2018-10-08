using AppointmentHub.Core.Common;
using AppointmentHub.Core.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AppointmentHub.Tests.Domain.Models
{
    [TestClass]
    public class NotificationTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Appointment")]
        public void AppointmentCanceled_WhenCalledWithNullAppointment_ArgumentNullExceptionExpected()
        {
            var notification = Notification.ApppointmentCanceled(null);
        }

        [TestMethod]
        public void AppointmentCanceled_WhenCalled_ShouldReturnedANotificationForACanceledAppointment()
        {
            var appointment = new Appointment();

            var notification = Notification.ApppointmentCanceled(appointment);

            notification.Type.Should().Be(NotificationType.AppointmentCanceled);
            notification.Appointment.Should().Be(appointment);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Appointment")]
        public void AppointmentCreated_WhenCalledWithNullAppointment_ArgumentNullExceptionExpected()
        {
            var notification = Notification.AppointmentCreated(null);
        }

        [TestMethod]
        public void AppointmentCreated_WhenCalled_ShouldReturnedANotificationForACreatedAppointment()
        {
            var appointment = new Appointment();

            var notification = Notification.AppointmentCreated(appointment);

            notification.Type.Should().Be(NotificationType.AppointmentCreated);
            notification.Appointment.Should().Be(appointment);
        }
    }
}

using AppointmentHub.Core.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AppointmentHub.Tests.Domain.Models
{
    [TestClass]
    public class UserAvailabilityTests
    {
        #region GetStartTime

        [TestMethod]
        public void GetStartTime_DateTimeHasMinutes_ReturnedTimeSpanOnlyHours()
        {
            var userAvailability = new UserAvailability()
            {
                DateTime = new DateTime(2018, 10, 6, 23, 10, 0)
            };

            var expectedTimespan = new TimeSpan(userAvailability.DateTime.Hour, 0, 0);

            var result = userAvailability.GetStartTime();

            result.Should().Be(expectedTimespan);
        }

        #endregion

        #region GetEndTime


        [TestMethod]
        public void GetEndTime_DateTimeHasMinutesTimeSpanHasMinutes_ReturnedTimeSpanOnlyHours()
        {
            var userAvailability = new UserAvailability()
            {
                DateTime = new DateTime(2018, 10, 6, 23, 10, 0),
                TimeSpan = new TimeSpan(4, 12, 5)
            };

            var expectedTimespan = new TimeSpan(userAvailability.DateTime.Add(userAvailability.TimeSpan).Hour, 0, 0);

            var result = userAvailability.GetEndTime();

            result.Should().Be(expectedTimespan);
        }

        #endregion
    }
}

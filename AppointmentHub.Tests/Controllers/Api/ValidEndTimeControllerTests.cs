using System;
using AppointmentHub.Controllers.Api;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppointmentHub.Tests.Controllers.Api
{
    [TestClass]
    public class ValidEndTimeControllerTests
    {

        private ValidEndTimeController _controller;


        [TestInitialize]
        public void TestInitialize()
        {
            _controller = new ValidEndTimeController();

        }


        [TestMethod]
        public void ValidEndTimes_StartTime2359_ReturnsEmpty()
        {
            var startTime = new TimeSpan(23, 59, 0);

            var result = _controller.ValidEndTimes(startTime);

            result.Should().BeEmpty();

        }

        [TestMethod]
        public void ValidEndTimes_StartTime0000_Returns23Times()
        {
            var startTime = new TimeSpan(0, 0, 0);

            var result = _controller.ValidEndTimes(startTime);

            result.Should().HaveCount(23);
        }

        [TestMethod]
        public void ValidEndTimes_StartTime1200_Returns11Times()
        {
            var startTime = new TimeSpan(12, 0, 0);

            var result = _controller.ValidEndTimes(startTime);

            result.Should().HaveCount(11);
        }
    }
}

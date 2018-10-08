using AppointmentHub.Core.Validators;
using AppointmentHub.Core.ViewModels;
using FluentAssertions;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace AppointmentHub.Tests.Domain.Validators
{
    [TestClass]
    public class AvailabilityFormValidatorTests
    {
        private AvailabilityFormValidator _validator;
        private AvailabilityFormViewModel _viewModel;

        [TestInitialize]
        public void TestInitialize()
        {
            _validator = new AvailabilityFormValidator();

            _viewModel = new AvailabilityFormViewModel()
            {
                StartTime = new TimeSpan(15, 50, 0),
                EndTime = new TimeSpan(16, 50, 0),
                Date = DateTime.Now.ToString("dd/MM/yyyy")
            };
        }

        [TestMethod]
        public void AvailabilityFormViewModel_Valid_IsValidTrue()
        {
            var results = _validator.Validate(_viewModel);

            results.IsValid.Should().Be(true);
            results.Errors.Should().BeEmpty();
        }

        [TestMethod]
        public void Date_IsNull_IsValidfalse()
        {
            _viewModel.Date = null;
            var results = _validator.Validate(_viewModel);
            var expectedFailuare = new ValidationFailure("Date", "'Date' must not be empty.");

            results.IsValid.Should().Be(false);
            results.Errors.Count.Should().Be(1);

            var failuareResults = results.Errors.First();
            failuareResults.PropertyName.Should().Be(expectedFailuare.PropertyName);
            failuareResults.ErrorMessage.Should().Be(expectedFailuare.ErrorMessage);
        }

        [TestMethod]
        public void Date_InvalidFormat_IsValidfalse()
        {
            _viewModel.Date = "test";
            var results = _validator.Validate(_viewModel);
            var expectedFailuare = new ValidationFailure("Date", "Date must be in the following format dd/MM/yyyy");

            results.IsValid.Should().Be(false);
            results.Errors.Count.Should().Be(1);

            var failuareResults = results.Errors.First();
            failuareResults.PropertyName.Should().Be(expectedFailuare.PropertyName);
            failuareResults.ErrorMessage.Should().Be(expectedFailuare.ErrorMessage);
        }

        [TestMethod]
        public void Date_PastDate_IsValidfalse()
        {
            _viewModel.Date = "11/09/2018";
            var results = _validator.Validate(_viewModel);
            var expectedFailuare = new ValidationFailure("Date", "Date must be for future appointments");

            results.IsValid.Should().Be(false);
            results.Errors.Count.Should().Be(1);

            var failuareResults = results.Errors.First();
            failuareResults.PropertyName.Should().Be(expectedFailuare.PropertyName);
            failuareResults.ErrorMessage.Should().Be(expectedFailuare.ErrorMessage);
        }

        [TestMethod]
        public void TimeSpan_LessThan1Hour_IsValidFalse()
        {
            _viewModel.StartTime = new TimeSpan(14, 0, 0);
            _viewModel.EndTime = new TimeSpan(13, 0, 0);


            var expectedFailuare = new ValidationFailure("TimeSpan", "Duration must be at least 1 hour");

            var results = _validator.Validate(_viewModel);

            results.IsValid.Should().Be(false);
            results.Errors.Count.Should().Be(1);

            var failuareResults = results.Errors.First();
            failuareResults.PropertyName.Should().Be(expectedFailuare.PropertyName);
            failuareResults.ErrorMessage.Should().Be(expectedFailuare.ErrorMessage);

        }

        [TestMethod]
        public void DateTime_NextDay_IsValidFalse()
        {
            _viewModel.EndTime = new TimeSpan(24, 50, 0);

            var expectedFailuare = new ValidationFailure("DateTime", "Appointment cannot got into the next day");

            var results = _validator.Validate(_viewModel);

            results.IsValid.Should().Be(false);
            results.Errors.Count.Should().Be(1);

            var failuareResults = results.Errors.First();
            failuareResults.PropertyName.Should().Be(expectedFailuare.PropertyName);
            failuareResults.ErrorMessage.Should().Be(expectedFailuare.ErrorMessage);

        }
    }
}

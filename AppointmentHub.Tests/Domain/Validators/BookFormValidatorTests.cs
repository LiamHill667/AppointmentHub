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
    public class BookFormValidatorTests
    {
        private BookFormValidator _validator;
        private BookFormViewModel _viewModel;

        [TestInitialize]
        public void TestInitialize()
        {
            _validator = new BookFormValidator();


            var userAvailability = new UserAvailabilityViewModel()
            {
                DateTime = DateTime.Parse("21/09/2018 15:50"),
                TimeSpan = new TimeSpan(2, 0, 0)
            };


            _viewModel = new BookFormViewModel()
            {
                Subject = "Test",
                Availability = userAvailability,
                StartTime = userAvailability.DateTime.TimeOfDay,
                EndTime = userAvailability.DateTime.TimeOfDay.Add(new TimeSpan(1, 0, 0))
            };
        }

        [TestMethod]
        public void BookFormViewModel_Valid_IsValidTrue()
        {
            var results = _validator.Validate(_viewModel);

            results.IsValid.Should().Be(true);
            results.Errors.Should().BeEmpty();
        }

        [TestMethod]
        public void Subject_Empty_IsValidFalse()
        {
            _viewModel.Subject = "";
            var expectedFailuare = new ValidationFailure("Subject", "Subject should not be empty");

            var results = _validator.Validate(_viewModel);

            results.IsValid.Should().Be(false);
            results.Errors.Count.Should().Be(1);

            var failuareResults = results.Errors.First();
            failuareResults.PropertyName.Should().Be(expectedFailuare.PropertyName);
            failuareResults.ErrorMessage.Should().Be(expectedFailuare.ErrorMessage);

        }

        [TestMethod]
        public void Subject_Over225Char_IsValidFalse()
        {
            _viewModel.Subject = "Lorem ipsum dolor sit amet," +
                                                " nonummy ligula volutpat hac integer nonummy." +
                                                " Suspendisse ultricies, congue etiam tellus," +
                                                " erat libero, nulla eleifend, mauris pellentesque." +
                                                " Suspendisse integer praesent vel, integer gravida mauris," +
                                                " fringilla vehicula lacinia non 1";

            var expectedFailuare = new ValidationFailure("Subject", "Subject should not be longer than 255 characters");

            var results = _validator.Validate(_viewModel);

            results.IsValid.Should().Be(false);
            results.Errors.Count.Should().Be(1);

            var failuareResults = results.Errors.First();
            failuareResults.PropertyName.Should().Be(expectedFailuare.PropertyName);
            failuareResults.ErrorMessage.Should().Be(expectedFailuare.ErrorMessage);

        }

        [TestMethod]
        public void AppointmentTimeSpan_LessThan1Hour_IsValidFalse()
        {
            _viewModel.EndTime = _viewModel.Availability.DateTime.TimeOfDay.Add(new TimeSpan(0, 1, 0));

            var expectedFailuare = new ValidationFailure("TimeSpan", "Length must be at least 1 hour");

            var results = _validator.Validate(_viewModel);

            results.IsValid.Should().Be(false);
            results.Errors.Count.Should().Be(1);

            var failuareResults = results.Errors.First();
            failuareResults.PropertyName.Should().Be(expectedFailuare.PropertyName);
            failuareResults.ErrorMessage.Should().Be(expectedFailuare.ErrorMessage);

        }

        [TestMethod]
        public void Time_AfterAvailability_IsValidFalse()
        {
            _viewModel.StartTime = _viewModel.Availability.DateTime
                                        .Add(_viewModel.Availability.TimeSpan.Add(new TimeSpan(1, 0, 0)))
                                        .TimeOfDay;
            _viewModel.EndTime = _viewModel.StartTime.Add(new TimeSpan(1, 0, 0));

            var expectedFailuare = new ValidationFailure("EndTime", "The appointment finishes later than the availability");
            var expectedFailuare2 = new ValidationFailure("StartTime", "The appointment does not start within the available time");

            var results = _validator.Validate(_viewModel);

            results.IsValid.Should().Be(false);
            results.Errors.Count.Should().Be(2);

            var failuareResults = results.Errors.First();
            failuareResults.PropertyName.Should().Be(expectedFailuare.PropertyName);
            failuareResults.ErrorMessage.Should().Be(expectedFailuare.ErrorMessage);

            var failuareResults2 = results.Errors[1];
            failuareResults2.PropertyName.Should().Be(expectedFailuare2.PropertyName);
            failuareResults2.ErrorMessage.Should().Be(expectedFailuare2.ErrorMessage);

        }

        [TestMethod]
        public void Time_BeforeAvailability_IsValidFalse()
        {
            _viewModel.StartTime = _viewModel.Availability.DateTime
                                .Subtract(new TimeSpan(1, 0, 0)).TimeOfDay;

            var expectedFailuare = new ValidationFailure("StartTime", "The appointment does not start within the available time");

            var results = _validator.Validate(_viewModel);

            results.IsValid.Should().Be(false);
            results.Errors.Count.Should().Be(1);

            var failuareResults = results.Errors.First();
            failuareResults.PropertyName.Should().Be(expectedFailuare.PropertyName);
            failuareResults.ErrorMessage.Should().Be(expectedFailuare.ErrorMessage);

        }

        [TestMethod]
        public void Appointment_AppointmentLengthTooLong_IsValidFalse()
        {
            _viewModel.EndTime = _viewModel.Availability.DateTime.TimeOfDay
                                       .Add(_viewModel.Availability.TimeSpan.Add(new TimeSpan(1, 0, 0)));

            var expectedFailuare = new ValidationFailure("EndTime", "The appointment finishes later than the availability");

            var results = _validator.Validate(_viewModel);

            results.IsValid.Should().Be(false);
            results.Errors.Count.Should().Be(1);

            var failuareResults = results.Errors.First();
            failuareResults.PropertyName.Should().Be(expectedFailuare.PropertyName);
            failuareResults.ErrorMessage.Should().Be(expectedFailuare.ErrorMessage);

        }
    }
}

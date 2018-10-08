using AppointmentHub.Core.Validators;
using AppointmentHub.Core.ViewModels;
using FluentAssertions;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace AppointmentHub.Tests.Domain.Validators
{
    [TestClass]
    public class MangeUserFormValidatorTests
    {
        private ManageUserFormValidator _validator;
        private ManageUserViewModel _viewModel;

        [TestInitialize]
        public void TestInitialize()
        {
            _validator = new ManageUserFormValidator();

            _viewModel = new ManageUserViewModel()
            {
                Name = "Billy Bob",
                Email = "user1@domain.com",
                Password = "Test123$"
            };
        }

        [TestMethod]
        public void ManageUserViewModel_Valid_IsValidTrue()
        {
            var results = _validator.Validate(_viewModel);

            results.IsValid.Should().Be(true);
            results.Errors.Should().BeEmpty();
        }

        [TestMethod]
        public void ManageUserViewModel_EmptyName_IsValidFalse()
        {
            _viewModel.Name = "";
            var expectedFailuare = new ValidationFailure("Name", "'Name' should not be empty.");

            var results = _validator.Validate(_viewModel);

            results.IsValid.Should().Be(false);
            results.Errors.Count().Should().Be(1);

            var failuareResults = results.Errors.First();
            failuareResults.PropertyName.Should().Be(expectedFailuare.PropertyName);
            failuareResults.ErrorMessage.Should().Be(expectedFailuare.ErrorMessage);
        }

        [TestMethod]
        public void ManageUserViewModel_EmptyEmail_IsValidFalse()
        {
            _viewModel.Email = "";
            var expectedFailuare = new ValidationFailure("Email", "'Email' should not be empty.");

            var results = _validator.Validate(_viewModel);

            results.IsValid.Should().Be(false);
            results.Errors.Count().Should().Be(1);

            var failuareResults = results.Errors.First();
            failuareResults.PropertyName.Should().Be(expectedFailuare.PropertyName);
            failuareResults.ErrorMessage.Should().Be(expectedFailuare.ErrorMessage);
        }

        [TestMethod]
        public void ManageUserViewModel_InvalidEmail_IsValidFalse()
        {
            _viewModel.Email = "Test";
            var expectedFailuare = new ValidationFailure("Email", "'Email' is not a valid email address.");

            var results = _validator.Validate(_viewModel);

            results.IsValid.Should().Be(false);
            results.Errors.Count().Should().Be(1);

            var failuareResults = results.Errors.First();
            failuareResults.PropertyName.Should().Be(expectedFailuare.PropertyName);
            failuareResults.ErrorMessage.Should().Be(expectedFailuare.ErrorMessage);
        }

        [TestMethod]
        public void ManageUserViewModel_EmptyPassword_IsValidFalse()
        {
            _viewModel.Password = "";
            var expectedFailuare = new ValidationFailure("Password", "'Password' should not be empty.");

            var results = _validator.Validate(_viewModel);

            results.IsValid.Should().Be(false);
            results.Errors.Count().Should().Be(1);

            var failuareResults = results.Errors.First();
            failuareResults.PropertyName.Should().Be(expectedFailuare.PropertyName);
            failuareResults.ErrorMessage.Should().Be(expectedFailuare.ErrorMessage);
        }

        [TestMethod]
        public void ManageUserViewModel_PasswordTooShort_IsValidFalse()
        {
            _viewModel.Password = "123";
            var expectedFailuare = new ValidationFailure("Password", "The length of 'Password' must be at least 6 characters. You entered 3 characters.");

            var results = _validator.Validate(_viewModel);

            results.IsValid.Should().Be(false);
            results.Errors.Count().Should().Be(1);

            var failuareResults = results.Errors.First();
            failuareResults.PropertyName.Should().Be(expectedFailuare.PropertyName);
            failuareResults.ErrorMessage.Should().Be(expectedFailuare.ErrorMessage);
        }

        [TestMethod]
        public void ManageUserViewModel_PasswordTooLong_IsValidFalse()
        {
            _viewModel.Password = "o50AChumia8RNrsbMnRUK7RQ" +
                                    "bsI4eKip6QUZcXpdSnkngUO1qygWHb8tcNi3GoHlaXW6" +
                                    "WIz53mqDyLoHPyQg5zocMLpkI7VrlENxwef";
            var expectedFailuare = new ValidationFailure("Password", "The length of 'Password' must be 100 characters or fewer. You entered 103 characters.");

            var results = _validator.Validate(_viewModel);

            results.IsValid.Should().Be(false);
            results.Errors.Count().Should().Be(1);

            var failuareResults = results.Errors.First();
            failuareResults.PropertyName.Should().Be(expectedFailuare.PropertyName);
            failuareResults.ErrorMessage.Should().Be(expectedFailuare.ErrorMessage);
        }
    }
}

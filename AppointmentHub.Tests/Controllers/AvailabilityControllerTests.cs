using AppointmentHub.App_Start;
using AppointmentHub.Controllers;
using AppointmentHub.Core.Models;
using AppointmentHub.Core.Services;
using AppointmentHub.Core.ViewModels;
using AppointmentHub.Tests.Extensions;
using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AppointmentHub.Tests.Controllers
{
    [TestClass]
    public class AvailabilityControllerTests
    {
        private AvailabilityController _controller;
        private Mock<IUserAvailabilityService> _mockService;
        private string _userId;


        [TestInitialize]
        public void TestInitialize()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.AddProfile<MappingProfile>());

            _mockService = new Mock<IUserAvailabilityService>();

            _controller = new AvailabilityController(_mockService.Object);
            _userId = "1";
            _controller.MockCurrentUser(_userId);

        }


        #region Index

        [TestMethod]
        public void Index_NoUserAvailabilitiesExist_ShouldReturnAvailabilityViewWithEmptyAvailability()
        {
            //Arrange
            int totalPages;
            IEnumerable<UserAvailability> availabilities = new UserAvailability[] { };
            _mockService.Setup(r => r.GetFutureUserAvailabilitiesPaged(_userId, 0, out totalPages, null))
                .Returns(availabilities);

            //Act
            var result = _controller.Index() as ViewResult;
            var model = result.Model as AvailabilityViewModel;

            //Assert
            result.Should().NotBeNull();
            result.ViewName.Should().Be("Availability");
            model.Availability.Should().HaveCount(0);
            model.Controller.Should().Be("Availability");
            model.Action.Should().Be("Index");
        }

        [TestMethod]
        public void Index_UserAvailabilitiesExist_ShouldReturnAvailabilityViewWithAvailabilities()
        {
            //Arrange
            int totalPages;
            IEnumerable<UserAvailability> availabilities = new UserAvailability[] { new UserAvailability() };
            _mockService.Setup(r => r.GetFutureUserAvailabilitiesPaged(_userId, 0, out totalPages, null))
                .Returns(availabilities);

            //Act
            var result = _controller.Index() as ViewResult;
            var model = result.Model as AvailabilityViewModel;

            //Assert
            result.Should().NotBeNull();
            result.ViewName.Should().Be("Availability");
            model.Availability.Should().HaveCount(1);
            model.Controller.Should().Be("Availability");
            model.Action.Should().Be("Index");

        }

        [TestMethod]
        public void Index_QueryNotSpecified_ViewModelHasQuery()
        {
            //Arrange
            int totalPages;
            string searchTerm = "Test";
            IEnumerable<UserAvailability> availabilities = new UserAvailability[] { };
            _mockService.Setup(r => r.GetFutureUserAvailabilitiesPaged(_userId, 0, out totalPages, searchTerm))
                .Returns(availabilities);

            //Act
            var result = _controller.Index() as ViewResult;
            var model = result.Model as AvailabilityViewModel;

            //Assert
            result.Should().NotBeNull();
            result.ViewName.Should().Be("Availability");
            model.SearchTerm.Should().Be(null);
            model.Controller.Should().Be("Availability");
            model.Action.Should().Be("Index");

        }

        [TestMethod]
        public void Index_QuerySpecified_ViewModelHasQuery()
        {
            //Arrange
            int totalPages;
            string searchTerm = "Test";
            IEnumerable<UserAvailability> availabilities = new UserAvailability[] { };
            _mockService.Setup(r => r.GetFutureUserAvailabilitiesPaged(_userId, 0, out totalPages, searchTerm))
                .Returns(availabilities);

            //Act
            var result = _controller.Index(searchTerm) as ViewResult;
            var model = result.Model as AvailabilityViewModel;

            //Assert
            result.Should().NotBeNull();
            result.ViewName.Should().Be("Availability");
            model.SearchTerm.Should().Be(searchTerm);
            model.Controller.Should().Be("Availability");
            model.Action.Should().Be("Index");

        }

        [TestMethod]
        public void Index_EmptyCall_ViewModelPageNum0()
        {
            //Arrange
            int totalPages;
            int pageNum = 0;
            IEnumerable<UserAvailability> availabilities = new UserAvailability[] { };
            _mockService.Setup(r => r.GetFutureUserAvailabilitiesPaged(_userId, 0, out totalPages, null))
                .Returns(availabilities);

            //Act
            var result = _controller.Index() as ViewResult;
            var model = result.Model as AvailabilityViewModel;

            //Assert
            result.Should().NotBeNull();
            result.ViewName.Should().Be("Availability");
            model.PageNum.Should().Be(pageNum);
            model.Controller.Should().Be("Availability");
            model.Action.Should().Be("Index");

        }

        [TestMethod]
        public void Index_PageNum_ViewModelPageNum()
        {
            //Arrange
            int totalPages;
            int pageNum = 1;
            IEnumerable<UserAvailability> availabilities = new UserAvailability[] { };
            _mockService.Setup(r => r.GetFutureUserAvailabilitiesPaged(_userId, pageNum, out totalPages, null))
                .Returns(availabilities);

            //Act
            var result = _controller.Index("", pageNum) as ViewResult;
            var model = result.Model as AvailabilityViewModel;

            //Assert
            result.Should().NotBeNull();
            result.ViewName.Should().Be("Availability");
            model.PageNum.Should().Be(pageNum);
            model.Controller.Should().Be("Availability");
            model.Action.Should().Be("Index");

        }

        #endregion

        #region Search

        [TestMethod]
        public void Search_ViewModel_RedirectToIndexWithQueryAndPageNum()
        {
            //Arrange
            var viewModel = new AvailabilityViewModel()
            {
                SearchTerm = "test",
                PageNum = 2
            };

            //Act
            var result = _controller.Search(viewModel) as RedirectToRouteResult;

            //Assert
            result.Should().NotBeNull();
            result.RouteValues.Should().HaveCount(3);

            var routeValue = result.RouteValues["action"];
            routeValue.Should().Be("Index");

            var routeValue1 = result.RouteValues["query"];
            routeValue1.Should().Be(viewModel.SearchTerm);

            var routeValue2 = result.RouteValues["pageNum"];
            routeValue2.Should().Be(viewModel.PageNum);

        }

        #endregion

        #region Create

        [TestMethod]
        public void Create__PageCall_ShouldReturnAvailabilityFormPage()
        {

            //Act
            var result = _controller.Create() as ViewResult;

            //Assert
            result.Should().NotBeNull();
            result.ViewName.Should().Be("AvailabilityForm");
            result.Model.Should().BeOfType<AvailabilityFormViewModel>();
        }

        [TestMethod]
        public void Create__InvalidModel_ShouldReloadPageWithErrors()
        {
            //Arrange
            var viewModel = new AvailabilityFormViewModel();
            _controller.ModelState.AddModelError("test", "test");

            //Act
            var result = _controller.Create(viewModel) as ViewResult;

            //Assert
            result.Should().NotBeNull();
            result.ViewName.Should().Be("AvailabilityForm");
            result.Model.Should().BeOfType<AvailabilityFormViewModel>();
            _controller.ModelState.IsValid.Should().Be(false);
        }

        [TestMethod]
        public void Create__ServiceFailuare_ServiceErrorAddedToModel()
        {
            //Arrange
            var expectedResult = new ServiceResult(false, "error message");
            _mockService.Setup(m => m.CreateAvailability(It.IsAny<UserAvailability>()))
                .Returns(expectedResult);

            var viewModel = new AvailabilityFormViewModel()
            {
                StartTime = new TimeSpan(15, 50, 0),
                EndTime = new TimeSpan(16, 50, 0),
                Date = DateTime.Now.ToString("dd/MM/yyyy")
            };

            //Act
            var result = _controller.Create(viewModel) as ViewResult;

            //Assert
            result.Should().NotBeNull();

            var serviceError = _controller.ModelState["User Availability"];
            serviceError.Errors.Should().HaveCount(1);
            serviceError.Errors[0].ErrorMessage.Should().Be(expectedResult.ErrorMessage);
        }

        [TestMethod]
        public void Create__ValidRequest_ShouldRedirectIndex()
        {
            //Arrange
            var expectedResult = new ServiceResult(true);
            _mockService.Setup(m => m.CreateAvailability(It.IsAny<UserAvailability>()))
                .Returns(expectedResult);

            var viewModel = new AvailabilityFormViewModel()
            {
                StartTime = new TimeSpan(15, 50, 0),
                EndTime = new TimeSpan(16, 50, 0),
                Date = DateTime.Now.ToString("dd/MM/yyyy")
            };

            //Act
            var result = _controller.Create(viewModel) as RedirectToRouteResult;

            //Assert
            result.Should().NotBeNull();
            result.RouteValues.Should().HaveCount(1);

            var routeValue = result.RouteValues["action"];
            routeValue.Should().Be("Index");
        }

        #endregion

        #region Edit

        [TestMethod]
        public void Edit__AvailabilityFoundAndUserPermission_ShouldReturnAvailabilityFormPage()
        {
            //Arrange
            var expectedResult = new UserAvailability()
            {
                Id = 1,
                UserId = _userId
            };
            _mockService.Setup(m => m.GetAvailability(It.IsAny<int>()))
                .Returns(expectedResult);

            //Act
            var result = _controller.Edit(expectedResult.Id) as ViewResult;
            var model = result.Model as AvailabilityFormViewModel;

            //Assert
            result.Should().NotBeNull();
            result.ViewName.Should().Be("AvailabilityForm");
            result.Model.Should().BeOfType<AvailabilityFormViewModel>();
            model.Id.Should().Be(expectedResult.Id);

        }

        [TestMethod]
        public void Edit__AvailabilityNotFound_ShouldReturnHttpNotFound()
        {
            //Arrange
            UserAvailability expectedResult = null;
            _mockService.Setup(m => m.GetAvailability(It.IsAny<int>()))
                .Returns(expectedResult);

            //Act
            var result = _controller.Edit(1) as ActionResult;

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<HttpNotFoundResult>();

        }

        [TestMethod]
        public void Edit__AvailabilityOfDifferentUser_ShouldReturnUnAuthorized()
        {
            //Arrange
            var expectedResult = new UserAvailability()
            {
                Id = 1,
                UserId = _userId + "-"
            };
            _mockService.Setup(m => m.GetAvailability(It.IsAny<int>()))
                .Returns(expectedResult);

            //Act
            var result = _controller.Edit(expectedResult.Id) as ActionResult;

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<HttpUnauthorizedResult>();

        }

        #endregion

        #region Update


        [TestMethod]
        public void Update__InvalidModel_ShouldReloadPageWithErrors()
        {
            //Arrange
            var viewModel = new AvailabilityFormViewModel()
            {
            };
            _controller.ModelState.AddModelError("test", "test");

            //Act
            var result = _controller.Update(viewModel) as ViewResult;

            //Assert
            result.Should().NotBeNull();
            result.ViewName.Should().Be("AvailabilityForm");
            result.Model.Should().BeOfType<AvailabilityFormViewModel>();
            _controller.ModelState.IsValid.Should().Be(false);
        }

        [TestMethod]
        public void Update__ServiceFailuare_ServiceErrorAddedToModel()
        {
            //Arrange
            var expectedResult = new ServiceResult(false, "error message");
            _mockService.Setup(m => m.CreateAvailability(It.IsAny<UserAvailability>()))
                .Returns(expectedResult);

            var viewModel = new AvailabilityFormViewModel()
            {
                StartTime = new TimeSpan(15, 50, 0),
                EndTime = new TimeSpan(16, 50, 0),
                Date = DateTime.Now.ToString("dd/MM/yyyy")
            };

            //Act
            var result = _controller.Create(viewModel) as ViewResult;

            //Assert
            result.Should().NotBeNull();

            var serviceError = _controller.ModelState["User Availability"];
            serviceError.Errors.Should().HaveCount(1);
            serviceError.Errors[0].ErrorMessage.Should().Be(expectedResult.ErrorMessage);
        }

        [TestMethod]
        public void Update__ValidRequest_ShouldRedirectIndex()
        {
            //Arrange
            var expectedResult = new ServiceResult(true);
            _mockService.Setup(m => m.CreateAvailability(It.IsAny<UserAvailability>()))
                .Returns(expectedResult);

            var viewModel = new AvailabilityFormViewModel()
            {
                StartTime = new TimeSpan(15, 50, 0),
                EndTime = new TimeSpan(16, 50, 0),
                Date = DateTime.Now.ToString("dd/MM/yyyy")
            };

            //Act
            var result = _controller.Create(viewModel) as RedirectToRouteResult;

            //Assert
            result.Should().NotBeNull();
            result.RouteValues.Should().HaveCount(1);

            var routeValue = result.RouteValues["action"];
            routeValue.Should().Be("Index");
        }

        #endregion
    }
}
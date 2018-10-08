using AppointmentHub.App_Start;
using AppointmentHub.Controllers;
using AppointmentHub.Core.Models;
using AppointmentHub.Core.Services;
using AppointmentHub.Core.ViewModels;
using AppointmentHub.Tests.Extensions;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AppointmentHub.Tests.Controllers
{
    [TestClass]
    public class AdminControllerTests
    {
        private AdminController _controller;
        private Mock<IUserService> _mockService;
        private string _userId;


        [TestInitialize]
        public void TestInitialize()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.AddProfile<MappingProfile>());

            _mockService = new Mock<IUserService>();
            var mockUserStore = new Mock<IUserStore<ApplicationUser, string>>();
            var userManager = new ApplicationUserManager(mockUserStore.Object);
            _controller = new AdminController(_mockService.Object, userManager);
            _userId = "1";
            _controller.MockCurrentUser(_userId);
        }

        #region Index

        [TestMethod]
        public void Index_PageNumSearchTermPassed_PageNumSearchTermInModel()
        {
            //Arrange
            int totalPages = 3;
            int pageNum = 2;
            string searchTerm = "test";
            IEnumerable<ApplicationUser> users = new ApplicationUser[] { new ApplicationUser() };
            _mockService.Setup(r => r.GetUsersPaged(pageNum, out totalPages, searchTerm))
               .Returns(users);

            //Act
            var result = _controller.Users(pageNum, searchTerm) as ViewResult;
            var model = result.Model as UsersViewModel;

            //Assert
            result.Should().NotBeNull();
            result.ViewName.Should().Be("");
            model.Users.Should().HaveCount(1);
            model.Controller.Should().Be("Admin");
            model.Action.Should().Be("UsersSearch");
            model.SearchTerm.Should().Be(searchTerm);
            model.PageNum.Should().Be(pageNum);

        }

        [TestMethod]
        public void Users_NoUsersExist_ShouldReturnAvailabilityViewWithEmptyAvailability()
        {
            //Arrange
            int totalPages = 3;
            IEnumerable<ApplicationUser> users = new ApplicationUser[] { };
            _mockService.Setup(r => r.GetUsersPaged(0, out totalPages, null))
                .Returns(users);

            //Act
            var result = _controller.Users() as ViewResult;
            var model = result.Model as UsersViewModel;

            //Assert
            result.Should().NotBeNull();
            result.ViewName.Should().Be("");
            model.Users.Should().HaveCount(0);
            model.SearchTerm.Should().BeNullOrEmpty();
            model.PageNum.Should().Be(0);
            model.Controller.Should().Be("Admin");
            model.Action.Should().Be("UsersSearch");
        }

        [TestMethod]
        public void Index_UserAvailabilitiesExist_ShouldReturnAvailabilityViewWithAvailabilities()
        {
            //Arrange
            int totalPages = 3;
            IEnumerable<ApplicationUser> users = new ApplicationUser[] { new ApplicationUser() };
            _mockService.Setup(r => r.GetUsersPaged(0, out totalPages, null))
               .Returns(users);

            //Act
            var result = _controller.Users() as ViewResult;
            var model = result.Model as UsersViewModel;

            //Assert
            result.Should().NotBeNull();
            result.ViewName.Should().Be("");
            model.Users.Should().HaveCount(1);
            model.Controller.Should().Be("Admin");
            model.Action.Should().Be("UsersSearch");

        }

        #endregion

        #region Search

        [TestMethod]
        public void UsersSearch_ViewModel_RedirectToUsersWithQueryAndPageNum()
        {
            //Arrange
            var viewModel = new UsersViewModel()
            {
                SearchTerm = "test",
                PageNum = 2
            };

            //Act
            var result = _controller.UsersSearch(viewModel) as RedirectToRouteResult;

            //Assert
            result.Should().NotBeNull();
            result.RouteValues.Should().HaveCount(3);

            var routeValue = result.RouteValues["action"];
            routeValue.Should().Be("Users");

            var routeValue1 = result.RouteValues["query"];
            routeValue1.Should().Be(viewModel.SearchTerm);

            var routeValue2 = result.RouteValues["pageNum"];
            routeValue2.Should().Be(viewModel.PageNum);

        }

        #endregion

        #region ManageUser

        [TestMethod]
        public void ManageUsers__PageCallNoId_ShouldReturnManageUsersEmptyManageUserViewModel()
        {

            //Act
            var result = _controller.ManageUser() as ViewResult;
            var model = result.Model as ManageUserViewModel;

            //Assert
            result.Should().NotBeNull();
            result.ViewName.Should().Be("");
            result.Model.Should().BeOfType<ManageUserViewModel>();
            model.Id.Should().BeNull();
        }

        [TestMethod]
        public void ManageUsers__PageCallIdUserNotFound_ShouldReturnHttpNotFound()
        {
            //Arrange
            ApplicationUser user = null;
            _mockService.Setup(m => m.GetUser(_userId)).Returns(user);

            //Act
            var result = _controller.ManageUser(_userId) as ActionResult;

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<HttpNotFoundResult>();
        }

        [TestMethod]
        public void ManageUsers__PageCallIdUserFound_ShouldReturnManageUsersPopulatedManageUserViewModel()
        {
            //Arrange
            ApplicationUser user = new ApplicationUser()
            {
                Id = _userId,
                Name = "name",
                Email = "email"
            };
            user.Roles.Add(new ApplicationUserRole() { Role = new ApplicationRole("test") });

            _mockService.Setup(m => m.GetUser(_userId)).Returns(user);

            //Act
            var result = _controller.ManageUser(_userId) as ViewResult;
            var model = result.Model as ManageUserViewModel;

            //Assert
            result.Should().NotBeNull();
            result.ViewName.Should().Be("");
            result.Model.Should().BeOfType<ManageUserViewModel>();
            model.Id.Should().Be(user.Id);
            model.Name.Should().Be(user.Name);
            model.Email.Should().Be(user.Email);
            model.Roles.Should().HaveCount(1);
            model.Roles.First().Name.Should().Be("test");
        }
        #endregion

        #region CreateUser

        [TestMethod]
        public void CreateUser_InvalidModel_ReloadPageWithModelErrors()
        {
            //Arrange
            var viewModel = new ManageUserViewModel()
            {
            };
            _controller.ModelState.AddModelError("", "");

            //Act
            var result = _controller.CreateUser(viewModel) as ViewResult;

            //Assert
            result.Should().NotBeNull();
            result.ViewName.Should().Be("ManageUser");
            result.Model.Should().BeOfType<ManageUserViewModel>();
            _controller.ModelState.IsValid.Should().Be(false);
        }

        [TestMethod]
        public void CreateUser_ServiceFailuare_AddErrorsToModel()
        {
            //Arrange
            var viewModel = new ManageUserViewModel()
            {
                Name = "Billy Bob",
                Email = "user1@domain.com",
                Password = "Test123$"
            };
            var expectedResult = new IdentityResult(new[] { "error message" });
            _mockService.Setup(m => m.CreateUser(viewModel.Name, viewModel.Email, viewModel.Email, viewModel.Roles1, It.IsAny<ApplicationUserManager>()))
                .Returns(expectedResult);

            //Act
            var result = _controller.CreateUser(viewModel) as ViewResult;

            //Assert
            result.Should().NotBeNull();
            _controller.ModelState.IsValid.Should().Be(false);

            var Error = _controller.ModelState[""];
            Error.Errors.Should().HaveCount(1);
            Error.Errors[0].ErrorMessage.Should().Be(expectedResult.Errors.First());
        }

        [TestMethod]
        public void CreateUser_ServiceSucceeded_RedirectToUsers()
        {
            //Arrange
            var viewModel = new ManageUserViewModel()
            {
                Name = "Billy Bob",
                Email = "user1@domain.com",
                Password = "Test123$"
            };
            var expectedResult = IdentityResult.Success;
            _mockService.Setup(m => m.CreateUser(viewModel.Name, viewModel.Email, viewModel.Email, viewModel.Roles1, It.IsAny<ApplicationUserManager>()))
                .Returns(expectedResult);

            //Act
            var result = _controller.CreateUser(viewModel) as RedirectToRouteResult;

            //Assert
            result.Should().NotBeNull();
            result.RouteValues.Should().HaveCount(1);

            var routeValue = result.RouteValues["action"];
            routeValue.Should().Be("Users");
        }

        #endregion

        #region UpdateUser

        [TestMethod]
        public void UpdateUser_InvalidModel_ReloadPageWithModelErrors()
        {
            //Arrange
            var viewModel = new ManageUserViewModel()
            {
            };
            _controller.ModelState.AddModelError("", "");

            //Act
            var result = _controller.UpdateUser(viewModel) as ViewResult;

            //Assert
            result.Should().NotBeNull();
            result.ViewName.Should().Be("ManageUser");
            result.Model.Should().BeOfType<ManageUserViewModel>();
            _controller.ModelState.IsValid.Should().Be(false);
        }

        [TestMethod]
        public void UpdateUser_ServiceFailuare_AddErrorsToModel()
        {
            //Arrange
            var viewModel = new ManageUserViewModel()
            {
                Id = _userId,
                Name = "Billy Bob",
                Email = "user1@domain.com",
                Password = "Test123$"
            };
            var expectedResult = new ServiceResult(false, "error message");
            _mockService.Setup(m => m.UpdateUser(_userId, viewModel.Name, viewModel.Email, viewModel.Roles1, It.IsAny<ApplicationUserManager>()))
                .Returns(expectedResult);

            //Act
            var result = _controller.UpdateUser(viewModel) as ViewResult;

            //Assert
            result.Should().NotBeNull();
            _controller.ModelState.IsValid.Should().Be(false);

            var Error = _controller.ModelState[""];
            Error.Errors.Should().HaveCount(1);
            Error.Errors[0].ErrorMessage.Should().Be(expectedResult.ErrorMessage);
        }

        [TestMethod]
        public void UpdateUser_ServiceSucceeded_RedirectToUsers()
        { //Arrange
            var viewModel = new ManageUserViewModel()
            {
                Id = _userId,
                Name = "Billy Bob",
                Email = "user1@domain.com",
                Password = "Test123$"
            };
            var expectedResult = new ServiceResult(true);
            _mockService.Setup(m => m.UpdateUser(_userId, viewModel.Name, viewModel.Email, viewModel.Roles1, It.IsAny<ApplicationUserManager>()))
                .Returns(expectedResult);

            //Act
            var result = _controller.UpdateUser(viewModel) as RedirectToRouteResult;

            //Assert
            result.Should().NotBeNull();
            result.RouteValues.Should().HaveCount(1);

            var routeValue = result.RouteValues["action"];
            routeValue.Should().Be("Users");
        }

        #endregion

    }
}

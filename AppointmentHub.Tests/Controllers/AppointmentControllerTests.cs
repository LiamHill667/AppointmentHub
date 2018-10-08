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
    public class AppointmentControllerTests
    {
        private AppointmentController _controller;
        private Mock<IAppointmentService> _mockAppointmentService;
        private Mock<IUserAvailabilityService> _mockAvailabilityService;
        private string _userId;


        [TestInitialize]
        public void TestInitialize()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.AddProfile<MappingProfile>());

            _mockAppointmentService = new Mock<IAppointmentService>();
            _mockAvailabilityService = new Mock<IUserAvailabilityService>();

            _controller = new AppointmentController(_mockAppointmentService.Object, _mockAvailabilityService.Object);
            _userId = "1";
            _controller.MockCurrentUser(_userId);

        }

        #region Index

        [TestMethod]
        public void Index_NoParamsPassed_pageNumSearchTermDefault()
        {
            //Arrange
            int totalPages;
            IEnumerable<UserAvailability> availabilities = new UserAvailability[] { };
            _mockAvailabilityService.Setup(r => r.GetFutureBookableAvailabilitiesPaged(_userId, 0, out totalPages, null))
                .Returns(availabilities);

            //Act
            var result = _controller.Index() as ViewResult;
            var model = result.Model as AvailabilityViewModel;

            //Assert
            result.Should().NotBeNull();
            result.ViewName.Should().Be("BookableAppointments");
            model.SearchTerm.Should().Be(null);
            model.PageNum.Should().Be(0);

        }

        [TestMethod]
        public void Index_NoUserAvailabilitiesExist_ShouldReturnAvailabilityViewWithEmptyAvailability()
        {
            //Arrange
            int totalPages;
            int pageNum = 2;
            string searchTerm = "test";
            IEnumerable<UserAvailability> availabilities = new UserAvailability[] { };
            _mockAvailabilityService.Setup(r => r.GetFutureBookableAvailabilitiesPaged(_userId, 0, out totalPages, null))
                .Returns(availabilities);

            //Act
            var result = _controller.Index(searchTerm, pageNum) as ViewResult;
            var model = result.Model as AvailabilityViewModel;

            //Assert
            result.Should().NotBeNull();
            result.ViewName.Should().Be("BookableAppointments");
            model.Availability.Should().HaveCount(0);
            model.SearchTerm.Should().Be(searchTerm);
            model.PageNum.Should().Be(pageNum);
            model.Controller.Should().Be("Appointment");
            model.Action.Should().Be("Index");

        }

        [TestMethod]
        public void Index_UserAvailabilitiesExist_ShouldReturnAvailabilityViewWithAvailabilities()
        {
            //Arrange
            int totalPages = 3;
            IEnumerable<UserAvailability> availabilities = new UserAvailability[] { new UserAvailability() };
            _mockAvailabilityService.Setup(r => r.GetFutureBookableAvailabilitiesPaged(_userId, 0, out totalPages, null))
                .Returns(availabilities);

            //Act
            var result = _controller.Index() as ViewResult;
            var model = result.Model as AvailabilityViewModel;

            //Assert
            result.Should().NotBeNull();
            result.ViewName.Should().Be("BookableAppointments");
            model.Availability.Should().HaveCount(1);
            model.TotalPages.Should().Be(totalPages);
            model.Controller.Should().Be("Appointment");
            model.Action.Should().Be("Index");

        }

        #endregion

        #region SearchBookable

        [TestMethod]
        public void SearchBookable_ViewModel_RedirectToIndexWithQueryAndPageNum()
        {
            //Arrange
            var viewModel = new AvailabilityViewModel()
            {
                SearchTerm = "test",
                PageNum = 2
            };

            //Act
            var result = _controller.SearchBookable(viewModel) as RedirectToRouteResult;

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

        #region Mine

        [TestMethod]
        public void Mine_SearchTermPageNumPassed_SearchTermPageNumInModel()
        {
            //Arrange
            int totalPages = 3;
            int pageNum = 2;
            string searchTerm = "test";
            IEnumerable<Appointment> appointments = new Appointment[] { };
            _mockAppointmentService.Setup(r => r.GetAppointmentsPaged(_userId, pageNum, out totalPages, searchTerm))
                .Returns(appointments);

            //Act
            var result = _controller.Mine(searchTerm, pageNum) as ViewResult;
            var model = result.Model as AppointmentListViewModel;

            //Assert
            result.Should().NotBeNull();
            result.ViewName.Should().Be("");
            model.Appointments.Should().HaveCount(0);
            model.SearchTerm.Should().Be(searchTerm);
            model.PageNum.Should().Be(pageNum);
            model.Controller.Should().Be("Appointment");
            model.Action.Should().Be("Mine");

        }

        [TestMethod]
        public void Mine_NoAppointmentsExist_ShouldReturnAppointmentViewWithEmptyAppointments()
        {
            //Arrange
            int totalPages = 3;
            IEnumerable<Appointment> appointments = new Appointment[] { };
            _mockAppointmentService.Setup(r => r.GetAppointmentsPaged(_userId, 0, out totalPages, null))
                .Returns(appointments);

            //Act
            var result = _controller.Mine() as ViewResult;
            var model = result.Model as AppointmentListViewModel;

            //Assert
            result.Should().NotBeNull();
            result.ViewName.Should().Be("");
            model.Appointments.Should().HaveCount(0);
            model.Controller.Should().Be("Appointment");
            model.Action.Should().Be("Mine");

        }

        [TestMethod]
        public void Mine_AppointmentsExist_ShouldReturnAppointmentViewWithAppointments()
        {
            //Arrange
            int totalPages = 3;
            IEnumerable<Appointment> appointments = new Appointment[] { new Appointment() };
            _mockAppointmentService.Setup(r => r.GetAppointmentsPaged(_userId, 0, out totalPages, null))
               .Returns(appointments);

            //Act
            var result = _controller.Mine() as ViewResult;
            var model = result.Model as AppointmentListViewModel;

            //Assert
            result.Should().NotBeNull();
            result.ViewName.Should().Be("");
            model.Appointments.Should().HaveCount(1);
            model.Controller.Should().Be("Appointment");
            model.Action.Should().Be("Mine");

        }

        #endregion

        #region SearchMine

        [TestMethod]
        public void SearchMine_ViewModel_RedirectToMineWithQueryAndPageNum()
        {
            //Arrange
            var viewModel = new AppointmentListViewModel()
            {
                SearchTerm = "test",
                PageNum = 2
            };

            //Act
            var result = _controller.SearchMine(viewModel) as RedirectToRouteResult;

            //Assert
            result.Should().NotBeNull();
            result.RouteValues.Should().HaveCount(3);

            var routeValue = result.RouteValues["action"];
            routeValue.Should().Be("Mine");

            var routeValue1 = result.RouteValues["query"];
            routeValue1.Should().Be(viewModel.SearchTerm);

            var routeValue2 = result.RouteValues["pageNum"];
            routeValue2.Should().Be(viewModel.PageNum);

        }

        #endregion

        #region Book

        [TestMethod]
        public void Book_AvailabilityExists_ReturnViewPopulatedBookFormViewModel()
        {
            //Arrange
            var availability = new UserAvailability()
            {
                Id = 1,
                DateTime = DateTime.Now
            };

            _mockAvailabilityService.Setup(m => m.GetAvailability(It.IsAny<int>())).Returns(availability);

            var types = new AppointmentType[] { new AppointmentType() };
            _mockAppointmentService.Setup(m => m.GetAppointmentTypes()).Returns(types);
            var times = new TimeSpan[] { new TimeSpan() };
            _mockAppointmentService.Setup(m => m.GetAppointmentStartTimes(availability)).Returns(times);
            _mockAppointmentService.Setup(m => m.GetAppointmentEndTimes(availability)).Returns(times);

            //Act
            var result = _controller.Book(1) as ViewResult;
            var model = result.Model as BookFormViewModel;

            //Assert
            result.Should().NotBeNull();
            result.ViewName.Should().Be("BookForm");
            model.Availability.Id.Should().Be(availability.Id);
            model.Types.Should().HaveCount(1);
            model.StartTime.Should().Be(availability.GetStartTime());
            model.SelectableStartTimes.Should().BeEquivalentTo(times);
            model.SelectableEndTimes.Should().BeEquivalentTo(times);

        }

        [TestMethod]
        public void Book_NoAvailabilityExists_ReturnHttpNotFound()
        {
            //Arrange
            UserAvailability availability = null;
            _mockAvailabilityService.Setup(m => m.GetAvailability(It.IsAny<int>())).Returns(availability);

            //Act
            var result = _controller.Book(1) as ActionResult;

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<HttpNotFoundResult>();

        }

        [TestMethod]
        public void Book__InvalidModel_ShouldReloadPageWithErrors()
        {
            //Arrange
            var availability = new UserAvailability()
            {
                Id = 113,
                DateTime = DateTime.Parse("21/09/2018 15:50"),
                TimeSpan = new TimeSpan(2, 0, 0)
            };
            _mockAvailabilityService.Setup(m => m.GetAvailability(It.IsAny<int>())).Returns(availability);

            var viewModel = new BookFormViewModel()
            {
                Availability = new UserAvailabilityViewModel()
                {
                    Id = 113,
                    DateTime = DateTime.Parse("21/09/2018 15:50"),
                    TimeSpan = new TimeSpan(2, 0, 0)
                }
            };

            _controller.ModelState.AddModelError("test", "test");

            //Act
            var result = _controller.Book(viewModel) as ViewResult;

            //Assert
            result.Should().NotBeNull();
            result.ViewName.Should().Be("BookForm");
            result.Model.Should().BeOfType<BookFormViewModel>();
            _controller.ModelState.IsValid.Should().Be(false);
        }

        [TestMethod]
        public void Book__ServiceFailuare_ServiceErrorAddedToModel()
        {
            //Arrange
            var userAvailability = new UserAvailabilityViewModel()
            {
                Id = 113,
                DateTime = DateTime.Parse("21/09/2018 15:50"),
                TimeSpan = new TimeSpan(2, 0, 0)
            };

            var viewModel = new BookFormViewModel()
            {
                Subject = "Test",
                Availability = userAvailability,
                StartTime = userAvailability.DateTime.TimeOfDay,
                EndTime = userAvailability.DateTime.TimeOfDay.Add(new TimeSpan(1, 0, 0)),
                Type = new AppointmentTypeViewModel() { Id = 132 }
            };

            var expectedResult = new ServiceResult(false, "error message");
            _mockAppointmentService.Setup(m => m.BookAppointment(viewModel.Subject, viewModel.DateTime,
                                          viewModel.TimeSpan, _userId, viewModel.Type.Id, viewModel.Availability.Id))
                                          .Returns(expectedResult);



            //Act
            var result = _controller.Book(viewModel) as ViewResult;

            //Assert
            result.Should().NotBeNull();

            var serviceError = _controller.ModelState["Appointment"];
            serviceError.Errors.Should().HaveCount(1);
            serviceError.Errors[0].ErrorMessage.Should().Be(expectedResult.ErrorMessage);
        }

        [TestMethod]
        public void Book__ValidRequest_ShouldRedirectIndex()
        {
            //Arrange
            var userAvailability = new UserAvailabilityViewModel()
            {
                Id = 113,
                DateTime = DateTime.Parse("21/09/2018 15:50"),
                TimeSpan = new TimeSpan(2, 0, 0)
            };

            var viewModel = new BookFormViewModel()
            {
                Subject = "Test",
                Availability = userAvailability,
                StartTime = userAvailability.DateTime.TimeOfDay,
                EndTime = userAvailability.DateTime.TimeOfDay.Add(new TimeSpan(1, 0, 0)),
                Type = new AppointmentTypeViewModel() { Id = 132 }
            };

            var expectedResult = new ServiceResult(true);
            _mockAppointmentService.Setup(m => m.BookAppointment(viewModel.Subject, viewModel.DateTime,
                                          viewModel.TimeSpan, _userId, viewModel.Type.Id, viewModel.Availability.Id))
                                          .Returns(expectedResult);

            //Act
            var result = _controller.Book(viewModel) as RedirectToRouteResult;

            //Assert
            result.Should().NotBeNull();
            result.RouteValues.Should().HaveCount(1);

            var routeValue = result.RouteValues["action"];
            routeValue.Should().Be("Index");
        }

        #endregion
    }
}

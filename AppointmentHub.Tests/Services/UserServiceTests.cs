using AppointmentHub.Core;
using AppointmentHub.Core.Common;
using AppointmentHub.Core.Models;
using AppointmentHub.Core.Repositories;
using AppointmentHub.Core.Services;
using FluentAssertions;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppointmentHub.Tests.Services
{
    [TestClass]
    public class UserServiceTests
    {
        private UserService _service;
        private Mock<IApplicationUserRepository> _mockRepository;
        private Mock<IUnitOfWork> _mockUoW;
        private Mock<IUserStore<ApplicationUser, string>> _mockUserStore;
        private Mock<IUserPasswordStore<ApplicationUser>> _mockUserPasswordStore;
        private Mock<IUserRoleStore<ApplicationUser>> _mockUserRoleStore;
        private ApplicationUserManager _userManager;
        private string _userId;


        [TestInitialize]
        public void TestInitialize()
        {
            _userId = "1";
            _mockRepository = new Mock<IApplicationUserRepository>();
            _mockUoW = new Mock<IUnitOfWork>();
            _mockUoW.SetupGet(u => u.ApplicationUsers).Returns(_mockRepository.Object);
            _mockUserStore = new Mock<IUserStore<ApplicationUser, string>>();
            _mockUserPasswordStore = _mockUserStore.As<IUserPasswordStore<ApplicationUser>>();
            _mockUserRoleStore = _mockUserStore.As<IUserRoleStore<ApplicationUser>>();

            _userManager = new ApplicationUserManager(_mockUserStore.Object);

            _service = new UserService(_mockUoW.Object);
        }

        #region Create

        [TestMethod]
        public void Create_IdentityFailuare_ReturnFailedIdentityResult()
        {
            var newUser = new ApplicationUser()
            {
                Name = "name",
                UserName = "user1@domain.com",
                Email = "user1@domain.com"
            };

            IEnumerable<string> roles = new string[] { "test" };
            string password = "test";
            IdentityResult returnedResult = new IdentityResult();

            _mockUserStore.Setup(m => m.CreateAsync(newUser))
                .Returns(Task.FromResult(returnedResult));


            var result = _service.CreateUser(newUser.Name, newUser.Email, password, roles, _userManager);

            result.Succeeded.Should().BeFalse();
        }


        [TestMethod]
        public void Create_IdentitySucceed_ReturnSuccessIdentityResult()
        {
            var newUser = new ApplicationUser()
            {
                Name = "name",
                UserName = "user1@domain.com",
                Email = "user1@domain.com"
            };

            IEnumerable<string> roles = AppRoles.GetRoleNames();
            string password = "Valid123%";
            _mockUserStore.Setup(m => m.CreateAsync(newUser))
                .Returns(Task.FromResult(IdentityResult.Success));

            _mockUserStore.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(newUser));
            IList<string> userRoles = new List<string>();
            _mockUserRoleStore.Setup(m => m.GetRolesAsync(newUser)).Returns(Task.FromResult(userRoles));

            var result = _service.CreateUser(newUser.Name, newUser.Email, password, roles, _userManager);

            result.Succeeded.Should().BeTrue();
        }

        #endregion

        #region GetUser


        [TestMethod]
        public void GetUser_WhenCalled_ReturnsUser()
        {
            //Arrange
            var user = new ApplicationUser();

            _mockRepository.Setup(m => m.GetById(user.Id))
                .Returns(user);

            //Act
            var result = _service.GetUser(user.Id);

            //Assert
            result.Should().NotBeNull();
            result.Should().Be(user);
        }


        #endregion

        #region GetUsersPaged

        [TestMethod]
        public void GetUsersPaged_WhenCalled_ReturnsUsers()
        {
            //Arrange
            var users = new ApplicationUser[] { new ApplicationUser() };
            int pageNum = 0;
            int totalPages = 3;
            string query = "test";

            _mockRepository.Setup(m => m.GetPagedUsers(pageNum, out totalPages, query, It.IsAny<int>()))
                .Returns(users);

            var result = _service.GetUsersPaged(pageNum, out totalPages, query);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(users);
        }

        #endregion

        #region UpdateUser

        [TestMethod]
        public void UpdateUser_UserNotFound_ReturnServiceFailuareResult()
        {
            ServiceResult expectedResult = new ServiceResult(false, "User not found");
            ApplicationUser user = null;
            _mockRepository.Setup(m => m.GetById(_userId))
                .Returns(user);


            var result = _service.UpdateUser(_userId, "test", "test", null, _userManager);

            result.Succeeded.Should().BeFalse();
            result.ErrorMessage.Should().Be(expectedResult.ErrorMessage);
        }

        //[TestMethod]
        //public void UpdateUser_IdentityFailuare_ReturnServiceFailuareResult()
        //{
        //    var newUser = new ApplicationUser()
        //    {
        //        Name = "name",
        //        UserName = "user1@domain.com",
        //        Email = "user1 com"
        //    };

        //    ServiceResult expectedResult = new ServiceResult(false, "User name user1 com is invalid, can only contain letters or digits.");
        //    _mockRepository.Setup(m => m.GetById(_userId))
        //        .Returns(newUser);

        //    IEnumerable<string> roles = new string[] { "test" };
        //    IdentityResult returnedResult = new IdentityResult();

        //    _mockUserStore.Setup(m => m.UpdateAsync(newUser))
        //        .Returns(Task.FromResult(returnedResult));

        //    var result = _service.UpdateUser(_userId, newUser.Name, newUser.Email, roles, _userManager);

        //    result.Succeeded.Should().BeFalse();
        //    result.ErrorMessage.Should().Be(expectedResult.ErrorMessage);
        //}


        [TestMethod]
        public void UpdateUser_IdentitySucceed_ReturnSuccessServiceResult()
        {
            var newUser = new ApplicationUser()
            {
                Name = "name",
                UserName = "user1@domain.com",
                Email = "user1@domain.com"
            };

            ServiceResult expectedResult = new ServiceResult(true);
            _mockRepository.Setup(m => m.GetById(_userId))
                .Returns(newUser);

            IEnumerable<string> roles = AppRoles.GetRoleNames();
            _mockUserStore.Setup(m => m.UpdateAsync(newUser))
                .Returns(Task.FromResult(IdentityResult.Success));

            _mockUserStore.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(newUser));
            IList<string> userRoles = new List<string>();
            _mockUserRoleStore.Setup(m => m.GetRolesAsync(newUser)).Returns(Task.FromResult(userRoles));

            var result = _service.UpdateUser(_userId, newUser.Name, newUser.Email, roles, _userManager);

            result.Succeeded.Should().BeTrue();
        }

        #endregion

    }
}

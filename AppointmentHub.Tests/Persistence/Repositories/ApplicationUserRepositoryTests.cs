using AppointmentHub.Core.Models;
using AppointmentHub.Persistence;
using AppointmentHub.Persistence.Repositories;
using AppointmentHub.Tests.Extensions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Data.Entity;
using System.Linq;

namespace AppointmentHub.Tests.Persistence.Repositories
{
    [TestClass]
    public class ApplicationUserRepositoryTests
    {
        private ApplicationUserRepository _repository;
        private Mock<DbSet<ApplicationUser>> _mockApplicationUsers;
        private Mock<IApplicationDbContext> _mockContext;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockApplicationUsers = new Mock<DbSet<ApplicationUser>>();
            _mockContext = new Mock<IApplicationDbContext>();

            _repository = new ApplicationUserRepository(_mockContext.Object);
        }

        [TestMethod]
        public void GetPagedUsers_WhenCalledNullSearch_ShouldReturn()
        {
            int pageNum = 0;
            string searchTerm = null;
            var user = new ApplicationUser();

            _mockApplicationUsers.SetSource(new[] { user });

            _mockContext.SetupGet(c => c.Users).Returns(_mockApplicationUsers.Object);

            var users = _repository.GetPagedUsers(pageNum, out int totalPages, searchTerm);

            users.Count().Should().Be(1);
            users.Should().Contain(user);
        }

        [TestMethod]
        public void GetPagedUsers_WhenCalledEmptySearch_ShouldReturn()
        {
            int pageNum = 0;
            string searchTerm = "";
            var user = new ApplicationUser() { };

            _mockApplicationUsers.SetSource(new[] { user });

            _mockContext.SetupGet(c => c.Users).Returns(_mockApplicationUsers.Object);

            var users = _repository.GetPagedUsers(pageNum, out int totalPages, searchTerm);

            users.Count().Should().Be(1);
            users.Should().Contain(user);
        }

        [TestMethod]
        public void GetPagedUsers_WhenCalledInvalidSearch_ShouldNotReturn()
        {
            int pageNum = 0;
            string searchTerm = "Test";
            var user = new ApplicationUser()
            {
                Name = "Bob"
            };

            _mockApplicationUsers.SetSource(new[] { user });

            _mockContext.SetupGet(c => c.Users).Returns(_mockApplicationUsers.Object);

            var users = _repository.GetPagedUsers(pageNum, out int totalPages, searchTerm);

            users.Should().BeEmpty();
        }

        [TestMethod]
        public void GetPagedUsers_WhenCalledValidSearch_ShouldReturn()
        {
            int pageNum = 0;
            string searchTerm = "Bob";
            var user = new ApplicationUser()
            {
                Name = searchTerm
            };

            _mockApplicationUsers.SetSource(new[] { user });

            _mockContext.SetupGet(c => c.Users).Returns(_mockApplicationUsers.Object);

            var users = _repository.GetPagedUsers(pageNum, out int totalPages, searchTerm);

            users.Count().Should().Be(1);
            users.Should().Contain(user);
        }
    }
}

using Moq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Mvc;

namespace AppointmentHub.Tests.Extensions
{
    public static class ControllerExtentions
    {
        public static void MockCurrentUser(this Controller controller, string userId)
        {
            var claim = new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", userId);
            var mockIdentity = new Mock<ClaimsIdentity>();
            mockIdentity.Setup(ci => ci.FindFirst(It.IsAny<string>())).Returns(claim);

            var mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.Setup(p => p.Identity).Returns(mockIdentity.Object);

            var mockControllerContext = new Mock<ControllerContext>();
            mockControllerContext.Setup(cc => cc.HttpContext.User).Returns(mockPrincipal.Object);

            controller.ControllerContext = mockControllerContext.Object;
        }


    }
}

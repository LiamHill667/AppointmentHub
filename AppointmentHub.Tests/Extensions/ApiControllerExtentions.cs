using System.Security.Claims;
using System.Security.Principal;
using System.Web.Http;

namespace AppointmentHub.Tests.Extensions
{
    public static class ApiControllerExtentions
    {
        public static void MockHttpContext(this ApiController controller, string userId, string username)
        {
            var identity = new GenericIdentity(username);
            identity.AddClaim(
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", username));
            identity.AddClaim(
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", userId));

            var principal = new GenericPrincipal(identity, null);

            controller.User = principal;
        }
    }

}

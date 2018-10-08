using AppointmentHub.Core.Common;
using AppointmentHub.Core.Services;
using Microsoft.AspNet.Identity;
using System.Web.Http;

namespace AppointmentHub.Controllers.Api
{
    [Authorize(Roles = AppRoles.Contact)]
    public class AvailabilityController : ApiController
    {
        private readonly IUserAvailabilityService _userAvailabilityService;

        public AvailabilityController(IUserAvailabilityService userAvailabilityService)
        {
            _userAvailabilityService = userAvailabilityService;

        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var result = _userAvailabilityService.DeleteAvailability(id, User.Identity.GetUserId());

            if (!result.Succeeded)
                return BadRequest(result.ErrorMessage);

            return Ok(id);
        }
    }
}

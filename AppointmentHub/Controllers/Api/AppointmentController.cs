using AppointmentHub.Core.Services;
using Microsoft.AspNet.Identity;
using System.Web.Http;

namespace AppointmentHub.Controllers.Api
{
    [Authorize]
    public class AppointmentController : ApiController
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost]
        public IHttpActionResult Cancel([FromBody] int appointmentId)
        {
            var result = _appointmentService.CancelAppointment(appointmentId, User.Identity.GetUserId());

            if (!result.Succeeded)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }

    }
}

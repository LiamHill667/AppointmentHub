using AppointmentHub.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace AppointmentHub.Controllers.Api
{
    public class ValidEndTimeController : ApiController
    {

        [HttpPost]
        public IEnumerable<TimeSpan> ValidEndTimes([FromBody]TimeSpan selectedStartTime)
        {
            return new TimeSpan().HoursOfTheDay().Where(t => t > selectedStartTime).ToList();
        }
    }
}

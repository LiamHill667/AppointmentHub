using System;
using System.Collections.Generic;

namespace AppointmentHub.Core.Extensions
{
    public static class TimeSpanExtentions
    {
        public static List<TimeSpan> HoursOfTheDay(this TimeSpan timeSpan)
        {
            List<TimeSpan> times = new List<TimeSpan>();

            for (int i = 0; i < 24; i++)
            {
                times.Add(new TimeSpan(i, 0, 0));
            }

            return times;
        }
    }
}
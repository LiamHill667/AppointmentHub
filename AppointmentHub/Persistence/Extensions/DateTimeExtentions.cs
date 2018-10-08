using System;
using System.Data.Entity;

namespace AppointmentHub.Persistence.Extensions
{
    public static class DateTimeExtentions
    {
        //[DbFunction("Edm", "TruncateTime")]
        //public static DateTime? TruncateTime(DateTime? dateValue)
        //{
        //    return dateValue?.Date;
        //}

        [DbFunction("Edm", "TruncateTime")]
        public static DateTime TruncateTime(this DateTime dateValue)
        {
            return dateValue.Date;
        }
    }
}
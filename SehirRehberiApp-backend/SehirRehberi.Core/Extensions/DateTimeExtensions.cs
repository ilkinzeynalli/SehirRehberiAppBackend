using System;
using System.Collections.Generic;
using System.Text;

namespace SehirRehberi.Core.Extensions
{
    public static class DatetimeExtensions
    {
        public static DateTime ConvertUtcToLocalTime(this DateTime datetime)
        {
            DateTime convertedDate = DateTime.SpecifyKind(
                            DateTime.Parse(datetime.ToString()),
                            DateTimeKind.Utc);

            return convertedDate.ToLocalTime();
        }
    }
}

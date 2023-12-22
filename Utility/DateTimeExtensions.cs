using System;

namespace WebPulse_WebManager.Utility
{
    public static class DateTimeExtensions
    {
        public static DateTime AddWeeks(this DateTime dateTime, int weeks)
        {
            return dateTime.AddDays(weeks * 7);
        }
    }
}
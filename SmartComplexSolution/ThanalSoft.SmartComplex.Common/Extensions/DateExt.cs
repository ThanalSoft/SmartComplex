using System;

namespace ThanalSoft.SmartComplex.Common.Extensions
{
    public static class DateExt
    {
        public static string ConvertWhen(this DateTime pWhen)
        {
            var timeSpan = DateTime.Now.Subtract(pWhen);

            if (timeSpan.TotalSeconds < 5)
                return "Seconds ago";

            if (timeSpan.TotalSeconds < 60)
                return $"{timeSpan.TotalSeconds:0} seconds ago";

            if (timeSpan.TotalMinutes < 5)
                return "Minutes ago";

            if (timeSpan.TotalMinutes < 60)
                return $"{timeSpan.TotalMinutes:0} minutes ago";

            if ((int) timeSpan.TotalHours < 2)
                return $"{(int) timeSpan.TotalHours:0} hour ago";

            if (timeSpan.TotalHours < 24)
                return $"{timeSpan.TotalHours:0} hours ago";

            if (timeSpan.TotalDays < 7)
                return pWhen.DayOfWeek.ToString();
            return pWhen.ToLongDateString();
        }
    }
}
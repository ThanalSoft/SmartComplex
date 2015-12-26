using System;

namespace ThanalSoft.SmartComplex.Web.Common.Extensions
{
    public static class DateExt
    {
        public static string GetDateForDisplay(this DateTime pDateTime)
        {
            return pDateTime.ToString("dd-MMM-yy");
        }
    }
}
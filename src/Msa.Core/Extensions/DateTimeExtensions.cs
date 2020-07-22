namespace Msa.Core.Extensions
{
    using System;

    public static class DateTimeExtensions
    {
        public static Boolean IsInUtc(this DateTime dateTime)
        {
            return dateTime.Kind == DateTimeKind.Utc;
        }

        public static String UtcTimestampString(this DateTime? dateTime)
        {
            if (!dateTime.HasValue)
            {
                return "<NA>";
            }

            return dateTime.Value.ToUniversalTime().ToString("O");
        }
    }
}
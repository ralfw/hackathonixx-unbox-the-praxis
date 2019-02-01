using System;
using x.common.Net.Extensions;

namespace unbox.frontend.helper
{
    internal static class TimeSlotStringMapper
    {
        public static string Map(DateTime selectedDate)
        {
            var userFriendlyString = selectedDate.ToHumanFriendlyDateString();
            return userFriendlyString;
        }

        internal static string Map(TimeSpan? startTimeSpan, TimeSpan? endTimeSpan)
        {
            if (startTimeSpan != null && endTimeSpan != null)
            {
                return ((TimeSpan)startTimeSpan).ToString(@"hh\:mm\") + "-" + ((TimeSpan)endTimeSpan).ToString(@"hh\:mm\") + " Uhr";
            }
            return null;
        }
    }
}

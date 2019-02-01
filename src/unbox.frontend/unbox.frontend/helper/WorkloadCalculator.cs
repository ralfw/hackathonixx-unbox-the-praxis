using System;
using System.Collections.Generic;
using unbox.frontend.enums;
using unbox.frontend.viewmodels.timeslotplan;

namespace unbox.frontend.helper
{
    internal static class WorkloadCalculator
    {
        public static double CalculateWorkloadFraction(IEnumerable<TimeSlotViewModel> timeSlots, TimeSpan? rangeStartTime = null, TimeSpan? rangeEndTime = null)
        {
            var workingHoursStart = new TimeSpan(8,0,0);
            var workingHoursEnd = new TimeSpan(18,0,0);
            if (rangeStartTime == null)
            {
                rangeStartTime = workingHoursStart;
            }

            if (rangeEndTime == null)
            {
                rangeEndTime = workingHoursEnd;
            }

            var workingMinutes = (rangeEndTime.Value - rangeStartTime.Value).TotalMinutes;
            var occupiedMinutes = 0.0d;
            foreach (var timeslot in timeSlots)
            {
                var slotStartTime = timeslot.ActualStartTime.TimeOfDay;
                var slotEndTime = timeslot.ActualStartTime.TimeOfDay + timeslot.Duration;
                if (slotStartTime < rangeStartTime)
                {
                    slotStartTime = rangeStartTime.Value;
                }
                if (slotEndTime > rangeEndTime)
                {
                    slotEndTime = rangeEndTime.Value;
                }

                if (slotEndTime > slotStartTime)
                {
                    var minutes = (slotEndTime - slotStartTime).TotalMinutes;
                    if (timeslot.IsBreak)
                    {
                        workingMinutes -= minutes;
                    }
                    else
                    {
                        occupiedMinutes += minutes;
                    }
                }
            }

            return occupiedMinutes / workingMinutes;
        }

        public static WorkloadEnum CalculateWorkload(IEnumerable<TimeSlotViewModel> timeSlots,
                                                     TimeSpan? rangeStartTime = null, 
                                                     TimeSpan? rangeEndTime = null)
        {
            var fraction = CalculateWorkloadFraction(timeSlots, rangeStartTime, rangeEndTime);
            if (double.IsNaN(fraction) || double.IsInfinity(fraction))
            {
                return WorkloadEnum.Blocked;
            }
            else if (fraction < 0.5)
            {
                return WorkloadEnum.Green;
            }
            else if (fraction < 0.8)
            {
                return WorkloadEnum.Yellow;
            }
            else
            {
                return WorkloadEnum.Red;
            }

        }

    }
}

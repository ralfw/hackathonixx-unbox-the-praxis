using System;
using System.Collections.Generic;
using unbox.frontend.helper;
using unbox.frontend.viewmodels.timeslotplan;
using x.common.WPF.ViewModel;

namespace unbox.frontend.viewmodels.nexttimeslots
{
    public class NextTimeSlotsViewModel : ViewModelBase
    {
        public List<DayViewModel> Days { get; }
        public List<TimeSlotViewModel> Timeslots { get; }

        private void UpdateWorkload()
        {
            foreach (var day in Days)
            {
                foreach (var hour in day.Hours)
                {
                    hour.Workload = WorkloadCalculator.CalculateWorkload(Timeslots, new TimeSpan(hour.HourInt, 0, 0),
                        new TimeSpan(hour.HourInt + 1, 0, 0));
                }
            }
        }


        public NextTimeSlotsViewModel(List<DayViewModel> days, List<TimeSlotViewModel> timeSlots)
        {
            Days = days;
            Timeslots = timeSlots;
            UpdateWorkload();
        }

    }
}

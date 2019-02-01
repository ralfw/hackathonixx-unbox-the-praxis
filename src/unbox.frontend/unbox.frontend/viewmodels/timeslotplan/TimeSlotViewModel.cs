using System;
using x.common.WPF.ViewModel;

namespace unbox.frontend.viewmodels.timeslotplan
{
    public class TimeSlotViewModel : ViewModelBase
    {
        public TimeSlotViewModel(DateTime slotStartTime, DateTime slotEndTime, DateTime actualStartTime, TimeSpan duration, bool isBreak = false)
        {
            SlotStartTime = slotStartTime;
            SlotEndTime = slotEndTime;
            ActualStartTime = actualStartTime;
            Duration = duration;
            IsBreak = isBreak;
        }

        public DateTime SlotStartTime { get; }
        public DateTime SlotEndTime { get; }
        public DateTime ActualStartTime { get; }
        public TimeSpan Duration { get; }

        public bool IsBreak { get; }

        private const double MinutesPerDay = 60 * 24;

        public double SlotStartRelation => SlotStartTime.TimeOfDay.TotalMinutes / MinutesPerDay;
        public double SlotEndRelation => SlotEndTime.TimeOfDay.TotalMinutes / MinutesPerDay;

        public double ActualStartRelation => ActualStartTime.TimeOfDay.TotalMinutes / MinutesPerDay;
        public double ActualEndRelation => ActualStartRelation + Duration.TotalMinutes / MinutesPerDay;

    }
}

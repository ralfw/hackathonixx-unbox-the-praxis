using System;
using System.Collections.Generic;
using System.Linq;
using unbox.frontend.helper;
using unbox.frontend.viewmodels.timeslotplan;
using x.common.WPF.Commands;
using x.common.WPF.ViewModel;

namespace unbox.frontend.viewmodels.timeslotcalendar
{
    public class CalendarViewModel : ViewModelBase
    {
        public List<MonthViewModel> Months { get; }
        public List<TimeSlotViewModel> TimeSlots { get; }
        private MonthViewModel _selectedMonth;

        public MonthViewModel SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                if (SetProperty(nameof(SelectedMonth), ref _selectedMonth, value))
                {
                    GoBackCommand.RaiseCanExecuteChanged();
                    GoForwardCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public RelayCommand GoBackCommand { get; private set; }
        public RelayCommand GoForwardCommand { get; private set; }

        private TimeSpan _startTime = new TimeSpan(8,0,0);

        public TimeSpan StartTime
        {
            get => _startTime;
            set
            {
                if (SetProperty(nameof(StartTime), ref _startTime, value))
                {
                    UpdateWorkload();
                }
            }
        }

        private TimeSpan _endTime = new TimeSpan(18,0,0);

        public TimeSpan EndTime
        {
            get => _endTime;
            set
            {
                if (SetProperty(nameof(EndTime), ref _endTime, value))
                {
                    UpdateWorkload();
                }
            }
        }

        private DateTime _selectedDate;

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (SetProperty(nameof(SelectedDate), ref _selectedDate, value))
                {
                    foreach (var month in Months)
                    {
                        if (month.MonthInt == value.Month && month.YearInt == value.Year)
                        {
                            foreach (var day in month.Days)
                            {
                                if (day.DayInt == SelectedDate.Day)
                                {
                                    day.IsSelected = true;
                                    SelectedMonth = month;
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }


        public CalendarViewModel(List<MonthViewModel> months, List<TimeSlotViewModel> timeSlots)
        {
            GoBackCommand = new RelayCommand(() => { GoMonth(-1); }, () => Months.IndexOf(SelectedMonth) > 0);
            GoForwardCommand = new RelayCommand(() => { GoMonth(1); }, () => Months.IndexOf(SelectedMonth) < Months.Count - 1);

            Months = months;
            foreach (var month in months)
            {
                var monthForClosure = month;
                month.DaySelected += sel =>
                {
                    foreach (var month2 in months)
                    {
                        month2.DeSelectAllExcept(sel);
                    }
                    SelectedDate = new DateTime(monthForClosure.YearInt, monthForClosure.MonthInt, sel.DayInt);
                };
            }

            TimeSlots = timeSlots;
            UpdateWorkload();
        }


        private void UpdateWorkload()
        {
            foreach (var month in Months)
            {
                foreach (var day in month.Days)
                {
                    if (day.DayInt != 0 && new DateTime(month.YearInt, month.MonthInt, day.DayInt).DayOfWeek != DayOfWeek.Sunday)
                    {
                        day.Workload = WorkloadCalculator.CalculateWorkload(
                            TimeSlots.Where(t =>
                                t.ActualStartTime.Year == month.YearInt && t.ActualStartTime.Month == month.MonthInt &&
                                t.ActualStartTime.Day == day.DayInt),
                            StartTime, EndTime);
                    }
                }
            }

        }

        private void GoMonth(int diff)
        {
            var newIndex = Months.IndexOf(SelectedMonth) + diff;
            if (newIndex >= 0 && newIndex < Months.Count)
            {
                SelectedMonth = Months[newIndex];
            }
        }

    }
}

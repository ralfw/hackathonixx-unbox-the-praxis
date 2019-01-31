using System;
using System.Collections.Generic;
using x.common.WPF.Commands;
using x.common.WPF.ViewModel;

namespace unbox.frontend.viewmodels.timeslotcalendar
{
    public class CalendarViewModel : ViewModelBase
    {
        public List<MonthViewModel> Months { get; set; }
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


        public CalendarViewModel(List<MonthViewModel> months)
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unbox.frontend.enums;
using x.common.WPF.ViewModel;

namespace unbox.frontend.viewmodels.timeslotcalendar
{
    public class MonthViewModel : ViewModelBase
    {
        public string Month { get;}
        public string Year { get; }
        public int MonthInt { get;}
        public int YearInt { get; }
        public List<DayViewModel> Days { get; set; }

        public event Action<DayViewModel> DaySelected;

        public MonthViewModel(int month, int year)
        {
            MonthInt = month;
            YearInt = year;

            var firstDayOfMonth = new DateTime(year, month, 1);
            var weekDaysBeforeFirstDay = GetWeekDay(firstDayOfMonth);
            var days = new List<DayViewModel>();
            for (var i = 0; i < weekDaysBeforeFirstDay; i++)
            {
                days.Add(new DayViewModel(0) { Workload = WorkloadEnum.Blocked});
            }

            var daysInMonth = new DateTime(year, month, 1).AddMonths(1).AddDays(-1).Day;
            for (var i = 1; i <= daysInMonth; i++)
            {
                days.Add(new DayViewModel(i) { Workload = WorkloadEnum.Green});
            }

            var remainingDaysInMonth = 6 - GetWeekDay(new DateTime(year, month, daysInMonth));
            for (var i = 0; i < remainingDaysInMonth; i++)
            {
                days.Add(new DayViewModel(0) { Workload = WorkloadEnum.Blocked});
            }

            foreach (var day in days)
            {
                day.DaySelected += sel => DaySelected?.Invoke(sel);
            }

            Days = days;
            Year = year.ToString();
            Month = firstDayOfMonth.ToString("MMMM");
        }

        private static int GetWeekDay(DateTime dateTime)
        {
            var weekDay = (int) dateTime.DayOfWeek;
            weekDay = weekDay - 1 == -1 ? 6 : weekDay - 1;
            return weekDay;
        }

        internal void DeSelectAllExcept(DayViewModel day)
        {
            foreach (var day2 in Days)
            {
                if (day2 != day)
                {
                    day2.IsSelected = false;
                }
            }
        }
    }
}

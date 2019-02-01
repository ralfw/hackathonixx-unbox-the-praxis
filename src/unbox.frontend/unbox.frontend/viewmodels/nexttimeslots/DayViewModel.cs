using System;
using System.Collections.Generic;
using x.common.WPF.ViewModel;

namespace unbox.frontend.viewmodels.nexttimeslots
{
    public class DayViewModel : ViewModelBase
    {
        public string Day { get; }

        public List<HourViewModel> Hours { get; set; }

        public DateTime Date { get; }

        public DayViewModel(string day, DateTime date)
        {
            Day = day.ToString();
            Date = date;
        }
    }
}

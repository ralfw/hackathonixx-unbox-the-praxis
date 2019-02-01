using System;
using System.Collections.Generic;
using x.common.WPF.ViewModel;
using x.common.Net.Extensions;

namespace unbox.frontend.viewmodels.nexttimeslots
{
    public class DayViewModel : ViewModelBase
    {
        public string Day { get; }
        public DateTime Date { get; }

        public List<HourViewModel> Hours { get; set; }

        public DayViewModel(DateTime date)
        {
            Date = date;
            Day = date.ToRelativeHumanFriendlyDateString();
        }
    }
}

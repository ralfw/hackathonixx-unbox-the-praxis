using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using x.common.WPF.ViewModel;

namespace unbox.frontend.viewmodels.nexttimeslots
{
    public class DayViewModel : ViewModelBase
    {
        public string Day { get; }

        public List<HourViewModel> Hours { get; set; }

        public DayViewModel(string day)
        {
            Day = day.ToString();
        }
    }
}

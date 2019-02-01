using System.Collections.Generic;
using System.Windows.Controls;
using unbox.frontend.viewmodels.timeslotplan;

namespace unbox.frontend.usercontrols
{
    /// <summary>
    /// Interaction logic for TimeSlotPlan.xaml
    /// </summary>
    public partial class TimeSlotPlan : UserControl
    {
        public List<HourViewModel> Hours { get; set; }

        public TimeSlotPlan()
        {
            var hours = new List<viewmodels.timeslotplan.HourViewModel>();
            for (var i = 0; i < 24; i++)
            {
                hours.Add(new viewmodels.timeslotplan.HourViewModel(i));
            }

            Hours = hours;
            InitializeComponent();

        }
    }
}

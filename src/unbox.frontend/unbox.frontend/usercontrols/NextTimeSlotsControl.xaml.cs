using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using unbox.frontend.enums;
using unbox.frontend.viewmodels.nexttimeslots;

namespace unbox.frontend.usercontrols
{
    /// <summary>
    /// Interaction logic for TimeSlotCalendarControl.xaml
    /// </summary>
    public partial class NextTimeSlotsControl : UserControl
    {
        public NextTimeSlotsControl()
        {
            InitializeComponent();
        }

        private void Bg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (((HourViewModel) ((FrameworkElement) sender).DataContext).Workload != WorkloadEnum.Blocked)
            {
                ((HourViewModel) ((FrameworkElement) sender).DataContext).IsPatientAvailable =
                    !((HourViewModel) ((FrameworkElement) sender).DataContext).IsPatientAvailable;
            }
        }
    }
}

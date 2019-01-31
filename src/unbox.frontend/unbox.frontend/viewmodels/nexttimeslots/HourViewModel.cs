using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using unbox.frontend.enums;
using x.common.WPF.ViewModel;

namespace unbox.frontend.viewmodels.nexttimeslots
{
    public class HourViewModel : ViewModelBase
    {
        public string Hour { get; }
        public int HourInt { get; }
        public WorkloadEnum Workload { get; set; }

        private bool _isPatientAvailable;

        public bool IsPatientAvailable
        {
            get => _isPatientAvailable;
            set => SetProperty(nameof(IsPatientAvailable), ref _isPatientAvailable, value);
        }

        public HourViewModel(int hour)
        {
            Hour = hour + ":00";
            HourInt = hour;
        }

    }
}

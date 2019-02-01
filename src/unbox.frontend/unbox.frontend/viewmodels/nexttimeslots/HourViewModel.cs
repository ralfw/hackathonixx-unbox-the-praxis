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
        public string NextHour { get; }
        public int HourInt { get; }

        private WorkloadEnum _workload;

        public WorkloadEnum Workload
        {
            get => _workload;
            set => SetProperty(nameof(Workload), ref _workload, value);
        }


        private bool _isPatientAvailable;

        public bool IsPatientAvailable
        {
            get => _isPatientAvailable;
            set => SetProperty(nameof(IsPatientAvailable), ref _isPatientAvailable, value);
        }

        public HourViewModel(int hour)
        {
            Hour = hour.ToString("00") + ":00";
            NextHour = "-" + (hour + 1).ToString("00") + ":00";
            HourInt = hour;
        }

    }
}

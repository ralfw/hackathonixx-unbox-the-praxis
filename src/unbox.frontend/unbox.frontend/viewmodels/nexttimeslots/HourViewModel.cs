using System;
using unbox.frontend.enums;
using x.common.Net.Extensions;
using x.common.WPF.ViewModel;

namespace unbox.frontend.viewmodels.nexttimeslots
{
    public class HourViewModel : ViewModelBase
    {
        public string Hour { get; }
        public string NextHour { get; }
        public int HourInt { get; }
        public WorkloadEnum Workload { get; set; }

        private bool _isPatientAvailable;

        public bool IsPatientAvailable
        {
            get => _isPatientAvailable;
            set
            {
                SetProperty(nameof(IsPatientAvailable), ref _isPatientAvailable, value);
                OnPatientAvailableChanged.CallIfNotNull();
            }
        }

        public Action OnPatientAvailableChanged { get; set; }

        public HourViewModel(int hour)
        {
            Hour = hour.ToString("00") + ":00";
            NextHour = "-" + (hour + 1).ToString("00") + ":00";
            HourInt = hour;
        }

    }
}

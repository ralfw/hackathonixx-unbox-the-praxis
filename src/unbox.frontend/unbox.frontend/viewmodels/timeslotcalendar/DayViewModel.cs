using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unbox.frontend.enums;
using unbox.frontend.helper;
using unbox.frontend.viewmodels.timeslotplan;
using x.common.WPF.ViewModel;

namespace unbox.frontend.viewmodels.timeslotcalendar
{
    public class DayViewModel : ViewModelBase
    {
        public string Day { get; }
        public int DayInt { get; }

        private WorkloadEnum _workload;

        public WorkloadEnum Workload
        {
            get => _workload;
            set => SetProperty(nameof(Workload), ref _workload, value);
        }


        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (SetProperty(nameof(IsSelected), ref _isSelected, value))
                {
                    if (value)
                    {
                        DaySelected?.Invoke(this);
                    }
                }
            }
        }

        public event Action<DayViewModel> DaySelected;

        public DayViewModel(int day, IEnumerable<TimeSlotViewModel> timeSlotsThatDay = null)
        {
            Day = day == 0 ? "" : day.ToString();
            DayInt = day;
            Workload = WorkloadEnum.Blocked;
        }

    }
}

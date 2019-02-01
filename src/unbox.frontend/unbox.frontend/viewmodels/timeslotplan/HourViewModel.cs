using x.common.WPF.ViewModel;

namespace unbox.frontend.viewmodels.timeslotplan
{
    public class HourViewModel : ViewModelBase
    {
        public int HourInt { get; }
        public string Hour { get; }

        public double TopRelation => HourInt / 24.0d;
        public double BottomRelation => TopRelation + 1.0d / 24.0d;

        public HourViewModel(int hour)
        {
            HourInt = hour;
            Hour = hour.ToString("00");
        }
    }
}

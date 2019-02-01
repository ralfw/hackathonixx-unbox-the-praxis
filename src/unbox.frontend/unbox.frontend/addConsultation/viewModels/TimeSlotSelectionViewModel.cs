using System;
using unbox.frontend.viewmodels.timeslotcalendar;
using x.common.WPF.Commands;
using x.common.WPF.ViewModel;

namespace unbox.frontend.addConsultation.viewModels
{
    public class TimeSlotSelectionViewModel : ViewModelBase
    {
        private CalendarViewModel _calenderViewModel;
        public CalendarViewModel CalenderViewModel
        {
            get => _calenderViewModel;
            set => SetProperty(nameof(CalenderViewModel), ref _calenderViewModel, value);
        }


        public RelayCommand AddTimeSlotCommand { get; set; }

        public TimeSlotSelectionViewModel(Action addTimeSlot)
        {
            AddTimeSlotCommand = new RelayCommand(addTimeSlot);
        }

    }
}

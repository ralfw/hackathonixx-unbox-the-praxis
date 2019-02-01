using System;
using System.Collections.Generic;
using unbox.frontend.viewmodels.nexttimeslots;
using x.common.WPF.Commands;
using x.common.WPF.ViewModel;

namespace unbox.frontend.addConsultation.viewModels
{
    public class AddConsultationViewModel : ViewModelBase
    {
        private string _patient;
        public string Patient
        {
            get => _patient;
            set => SetProperty(nameof(Patient), ref _patient, value);
        }

        private int _requestesDuration;
        public int RequestedDuration
        {
            get => _requestesDuration;
            set => SetProperty(nameof(RequestedDuration), ref _requestesDuration, value);
        }

        private string _requestedDateString;
        public string RequestedDateString
        {
            get => _requestedDateString;
            set => SetProperty(nameof(RequestedDateString), ref _requestedDateString, value);
        }

        private string _requestedTimeSlotString;
        public string RequestedTimeSlotString
        {
            get => _requestedTimeSlotString;
            set => SetProperty(nameof(RequestedTimeSlotString), ref _requestedTimeSlotString, value);
        }

        public DateTime RequestedStart { get; set; }
        public DateTime RequestedEnd { get; set; }


        public List<DayViewModel> Days { get; set; }

        public RelayCommand AddConsultationCommand { get; set; }

        public RelayCommand AddTimeSlotCommand { get; set; }

        public RelayCommand CloseCommand { get; set; }

        public AddConsultationViewModel(Action addConsultation, Action onHasToShowSelectionTimeSlots, Action onCloseRequest)
        {
            AddConsultationCommand = new RelayCommand(addConsultation);
            AddTimeSlotCommand = new RelayCommand(onHasToShowSelectionTimeSlots);
            CloseCommand = new RelayCommand(onCloseRequest);           
        }

    }
}

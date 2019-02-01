using System;
using System.Collections.Generic;
using unbox.frontend.viewmodels.nexttimeslots;
using x.common.Net.Extensions;
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

        private int _requestedDuration;
        public int RequestedDuration
        {
            get => _requestedDuration;
            set => SetProperty(nameof(RequestedDuration), ref _requestedDuration, value);
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

        private bool _isUrgent;
        public bool IsUrgent
        {
            get => _isUrgent;
            set => SetProperty(nameof(IsUrgent), ref _isUrgent, value);
        }

        public List<DayViewModel> Days { get; set; }

        public RelayCommand AddConsultationCommand { get; set; }

        public RelayCommand AddTimeSlotCommand { get; set; }

        public RelayCommand RemoveRequestedTimeSpanCommand { get; set; }

        public RelayCommand CloseCommand { get; set; }

        private Action _onHasToShowSelectionTimeSlots;

        public AddConsultationViewModel(Action addConsultation, Action onHasToShowSelectionTimeSlots, Action onCloseRequest)
        {
            _onHasToShowSelectionTimeSlots = onHasToShowSelectionTimeSlots;

            AddConsultationCommand = new RelayCommand(addConsultation);
            AddTimeSlotCommand = new RelayCommand(OnHasToShowSelectionTimeSlots);
            CloseCommand = new RelayCommand(onCloseRequest);
            RemoveRequestedTimeSpanCommand = new RelayCommand(RemoveRequestedTimeSpan);

            IsUrgent = true;
        }

        private void OnHasToShowSelectionTimeSlots()
        {
            IsUrgent = false;
            _onHasToShowSelectionTimeSlots.CallIfNotNull();
        }

        public void RemoveRequestedTimeSpan()
        {
            RequestedStart = DateTime.MinValue;
            RequestedEnd = DateTime.MinValue;
            RequestedDateString = null;
            RequestedTimeSlotString = null;
        }

    }
}

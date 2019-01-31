﻿using System;
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

        public RelayCommand AddConsultationCommand { get; set; }

        public RelayCommand AddTimeSlotCommand { get; set; }

        public AddConsultationViewModel(Action addConsultation, Action onHasToShowSelectionTimeSlots)
        {
            AddConsultationCommand = new RelayCommand(addConsultation);
            AddTimeSlotCommand = new RelayCommand(onHasToShowSelectionTimeSlots);
        }

    }
}
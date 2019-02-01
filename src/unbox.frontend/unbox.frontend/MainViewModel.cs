using System;
using System.Collections.Generic;
using unbox.contracts;
using unbox.frontend.addConsultation;
using unbox.frontend.viewmodels.timeslotplan;
using x.common.WPF.Commands;
using x.common.WPF.ViewModel;

namespace unbox.frontend
{
    public class MainViewModel : ViewModelBase
    {
        private List<TimeSlotViewModel> _timeSlots;
        public List<TimeSlotViewModel> TimeSlots
        {
            get => _timeSlots;
            set => SetProperty(nameof(TimeSlots), ref _timeSlots, value);
        }
        private List<HourViewModel> _hours;

        public List<HourViewModel> Hours
        {
            get => _hours;
            set => SetProperty(nameof(Hours), ref _hours, value);
        }



        public RelayCommand ShowAddConsultationCommand { get; set; }

        private AddConsultationUi _addConsultationUi;


        public MainViewModel(IBackendRequestHandler backendRequestHandler)
        {
            _addConsultationUi = new AddConsultationUi(backendRequestHandler, () => OnConsultationAdded(backendRequestHandler));
            ShowAddConsultationCommand = new RelayCommand(OnShowAddConsultationWindowRequest);

            //GenerateTestData();
            FillSchedule(null, DateTime.Today);
        }

        private void OnConsultationAdded(IBackendRequestHandler backendRequestHandler)
        {
            var query = new CurrentPlanQuery();
            query.Date = DateTime.Today;
            var resultPlan = backendRequestHandler.Handle(query);

            FillSchedule(resultPlan, query.Date);
        }

        private void FillSchedule(CurrentPlanResult resultPlan, DateTime queryDate)
        {
            var timeSlots = new List<TimeSlotViewModel>();
            timeSlots.Add(new TimeSlotViewModel(queryDate + new TimeSpan(12,0,0),queryDate + new TimeSpan(13,0,0), queryDate + new TimeSpan(12,0,0), new TimeSpan(1,0,0), true));
            if (resultPlan != null)
            {
                foreach (var result in resultPlan.Schedule)
                {
                    timeSlots.Add(new TimeSlotViewModel(result.RequestedTimeslot.Start, result.RequestedTimeslot.End,
                        result.AssignedTimeslotStart, result.RequestedTimeslot.Duration));
                }
            }


            TimeSlots = timeSlots;
            Hours = new List<viewmodels.timeslotplan.HourViewModel>();
            for (var i = 0; i < 24; i++)
            {
                Hours.Add(new viewmodels.timeslotplan.HourViewModel(i));
            }
        }

        private void OnShowAddConsultationWindowRequest()
        {
            _addConsultationUi.ShowConsultationUi();
        }

        private void GenerateTestData()
        {
            TimeSlots = new List<TimeSlotViewModel>
            {
                new TimeSlotViewModel(new DateTime(2018,05,12,08,00,00), new DateTime(2018,05,12,12,00,00), new DateTime(2018,05,12,09,00,00), new TimeSpan(1,00,00), true),
                new TimeSlotViewModel(new DateTime(2018,05,12,07,00,00), new DateTime(2018,05,12,13,00,00), new DateTime(2018,05,12,10,00,00), new TimeSpan(1,30,00))
            };
            Hours = new List<viewmodels.timeslotplan.HourViewModel>();
            for (var i = 0; i < 24; i++)
            {
                Hours.Add(new viewmodels.timeslotplan.HourViewModel(i));
            }
        }

        
    }
}

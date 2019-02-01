using System;
using System.Collections.Generic;
using unbox.contracts;
using unbox.frontend.addConsultation.view;
using unbox.frontend.addConsultation.viewModels;
using unbox.frontend.enums;
using unbox.frontend.helper;
using unbox.frontend.viewmodels.nexttimeslots;
using unbox.frontend.viewmodels.timeslotcalendar;
using x.common.Net.Extensions;
using unbox.frontend.viewmodels.timeslotplan;
using x.common.WPF.Dialogs;
using DayViewModel = unbox.frontend.viewmodels.nexttimeslots.DayViewModel;
using HourViewModel = unbox.frontend.viewmodels.nexttimeslots.HourViewModel;

namespace unbox.frontend.addConsultation
{
    internal class AddConsultationUi
    {
        private AddConsultationViewModel _viewModel;
        private AddConsultationWindow _window;

        private TimeSlotSelectionViewModel _timeSlotSelectionViewModel;
        private TimeSlotSelectionWindow _timeSlotSelectionWindow;

        private IBackendRequestHandler _backendRequestHandler;

        private Action _onConsultationAdded;

        internal AddConsultationUi(IBackendRequestHandler backendRequestHandler, Action onConsultationAdded)
        {
            _backendRequestHandler = backendRequestHandler;
            _onConsultationAdded = onConsultationAdded;
        }

        internal void ShowConsultationUi()
        {
            _viewModel = new AddConsultationViewModel(OnAddConsultationRequest, OnHasToShowSelectionTimeSlots, OnCloseRequest);
            _viewModel.Patient = "Test, Theo";
            _viewModel.RequestedDuration = 15;
            SetTestDataAddConsultationViewModel();

            _window = new AddConsultationWindow();
            _window.DataContext = _viewModel;
            _window.Show();
        }

        private void OnCloseRequest()
        {
            _window.Close();
        }

        private void SetTestDataAddConsultationViewModel()
        {
            var hoursToday = new List<HourViewModel>();
            for(int i = Math.Max(DateTime.Now.Hour + 1, 8); i < 18; i++)
            {
                hoursToday.Add(new HourViewModel(i){ OnPatientAvailableChanged = OnPatientAvailableChanged });
            }
            var hoursTomorrow = new List<HourViewModel>();
            for(int i = 8; i < 18; i++)
            {
                hoursTomorrow.Add(new HourViewModel(i) { OnPatientAvailableChanged = OnPatientAvailableChanged });
            }
            _viewModel.DataContextNextCalendar = new NextTimeSlotsViewModel(
            new List<DayViewModel>()
            {
                new DayViewModel(DateTime.Today)
                {
                    Hours = hoursToday
                },
                new DayViewModel(DateTime.Today.AddDays(1))
                {
                    Hours = hoursTomorrow
                }
            }, GetAllTimeSlots(2));
        }

       

        private void OnPatientAvailableChanged()
        {
            _viewModel.IsUrgent = true;
        }


        private void OnAddConsultationRequest()
        {
            var success = _backendRequestHandler.Handle(DtoMapper.Map(_viewModel));
            if (success)
            {
                _onConsultationAdded.CallIfNotNull();
                _window.Close();
            }
            else
            {
                MessageDialog.CreateNotification("Konsultation konnte nicht angelegt werden",
                    "Bitte geben Sie ggf. einen anderen Verfügbarkeitszeitraum an", "x.smartplan").Show(_window);
            }
        }
        private void OnHasToShowSelectionTimeSlots()
        {
            _timeSlotSelectionViewModel = new TimeSlotSelectionViewModel(AddTimeSlot);
            SetTestDataTimeSlotSelection();
            _timeSlotSelectionViewModel.CalenderViewModel.StartTime = new TimeSpan(8,0,0);
            _timeSlotSelectionViewModel.CalenderViewModel.EndTime = new TimeSpan(18, 0, 0);
            _timeSlotSelectionWindow = new TimeSlotSelectionWindow {DataContext = _timeSlotSelectionViewModel};
            _timeSlotSelectionWindow.ShowDialog();
        }

        private void SetTestDataTimeSlotSelection()
        {
            /*var dummySlots = new List<TimeSlotViewModel>();
            dummySlots.Add(new TimeSlotViewModel(new DateTime(2019,2,2,8,0,0), new DateTime(2019,2,2,20,0,0), new DateTime(2019,2,2,8,0,0), new TimeSpan(6,0,0)));*/
            var months = new List<MonthViewModel>
            {
                new MonthViewModel(1,2019),
                new MonthViewModel(2,2019),
                new MonthViewModel(3,2019),
                new MonthViewModel(4,2019),
                new MonthViewModel(5,2019),
                new MonthViewModel(6,2019),
                new MonthViewModel(7,2019),
                new MonthViewModel(8,2019),
                new MonthViewModel(9,2019),
                new MonthViewModel(10,2019),
            };
            var calendar = new CalendarViewModel(months, GetAllTimeSlots(365))
            {
                SelectedMonth = months[2],
                SelectedDate = DateTime.Today
            };        
            _timeSlotSelectionViewModel.CalenderViewModel = calendar;
        }


        private void AddTimeSlot()
        {
            SetRequestedSlot();
            _timeSlotSelectionWindow.Close();
        }

        private void SetRequestedSlot()
        {
            var date = _timeSlotSelectionViewModel.CalenderViewModel.SelectedDate;
            _viewModel.RequestedDateString = TimeSlotStringMapper.Map(date);
            SetRequestedTimeSlot(date, _timeSlotSelectionViewModel.CalenderViewModel.StartTime, _timeSlotSelectionViewModel.CalenderViewModel.EndTime);
        }

        private void SetRequestedTimeSlot(DateTime date, TimeSpan? start, TimeSpan? end)
        {
            _viewModel.RequestedTimeSlotString = TimeSlotStringMapper.Map(start, end);
            if (start != null)
            {
                _viewModel.RequestedStart = (DateTime) (date + start);
            }

            if (end != null)
            {
                _viewModel.RequestedEnd = (DateTime) (date + end);
            }
        }


        private List<TimeSlotViewModel> GetAllTimeSlots(int nextDays)
        {
            var list = new List<TimeSlotViewModel>();

            var query = new CurrentPlanQuery();
            for (int i = 0; i < nextDays; i++)
            {
                query.Date = DateTime.Today.AddDays(i);
                var resultPlan = _backendRequestHandler.Handle(query);
                if (resultPlan != null)
                {
                    list.AddRange(GetSlots(resultPlan));
                }
            }

            return list;
        }

        private List<TimeSlotViewModel> GetSlots(CurrentPlanResult resultPlan)
        {
            var timeSlots = new List<TimeSlotViewModel>();
            foreach (var result in resultPlan.Schedule)
            {
                timeSlots.Add(new TimeSlotViewModel(result.RequestedTimeslot.Start, result.RequestedTimeslot.End,
                    result.AssignedTimeslotStart, result.RequestedTimeslot.Duration));
            }

            return timeSlots;
        }
    }
}

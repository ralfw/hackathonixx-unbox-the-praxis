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
using DayViewModel = unbox.frontend.viewmodels.nexttimeslots.DayViewModel;

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
            _viewModel.Patient = "1";
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
            _viewModel.Days = new List<DayViewModel>()
            {
                new DayViewModel("heute", DateTime.Today)
                {
                    Hours = new List<HourViewModel>
                    {
                        new HourViewModel(14)
                        {
                            IsPatientAvailable = false,
                            Workload = WorkloadEnum.Green
                        },
                        new HourViewModel(15)
                        {
                            IsPatientAvailable = false,
                            Workload = WorkloadEnum.Green,
                            OnPatientAvailableChanged = OnPatientAvailableChanged
                        },
                        new HourViewModel(16)
                        {
                            IsPatientAvailable = false,
                            Workload = WorkloadEnum.Red,
                            OnPatientAvailableChanged = OnPatientAvailableChanged
                        },
                        new HourViewModel(17)
                        {
                            IsPatientAvailable = false,
                            Workload = WorkloadEnum.Blocked,
                            OnPatientAvailableChanged = OnPatientAvailableChanged
                        },
                        new HourViewModel(18)
                        {
                            IsPatientAvailable = false,
                            Workload = WorkloadEnum.Yellow,
                            OnPatientAvailableChanged = OnPatientAvailableChanged
                        },
                    }
                },
                new DayViewModel("morgen", DateTime.Today.AddDays(1))
                {
                    Hours = new List<HourViewModel>
                    {
                        new HourViewModel(8)
                        {
                            IsPatientAvailable = false,
                            Workload = WorkloadEnum.Green,
                            OnPatientAvailableChanged = OnPatientAvailableChanged
                        },
                        new HourViewModel(9)
                        {
                            IsPatientAvailable = false,
                            Workload = WorkloadEnum.Green,
                            OnPatientAvailableChanged = OnPatientAvailableChanged
                        },
                        new HourViewModel(10)
                        {
                            IsPatientAvailable = false,
                            Workload = WorkloadEnum.Red,
                            OnPatientAvailableChanged = OnPatientAvailableChanged
                        },
                        new HourViewModel(11)
                        {
                            IsPatientAvailable = false,
                            Workload = WorkloadEnum.Blocked,
                            OnPatientAvailableChanged = OnPatientAvailableChanged
                        },
                        new HourViewModel(12)
                        {
                            IsPatientAvailable = false,
                            Workload = WorkloadEnum.Yellow,
                            OnPatientAvailableChanged = OnPatientAvailableChanged
                        },
                    }
                }
            };
        }

        private void OnPatientAvailableChanged()
        {
            _viewModel.IsUrgent = true;
        }


        private void OnAddConsultationRequest()
        {
            _backendRequestHandler.Handle(DtoMapper.Map(_viewModel));
            _onConsultationAdded.CallIfNotNull();
            _window.Close();
        }
        private void OnHasToShowSelectionTimeSlots()
        {
            _timeSlotSelectionViewModel = new TimeSlotSelectionViewModel(AddTimeSlot);
            SetTestDataTimeSlotSelection();
            _timeSlotSelectionViewModel.StartTimeSpan = new TimeSpan(0,0,0);
            _timeSlotSelectionViewModel.EndTimeSpan = new TimeSpan(23, 59, 0);
            _timeSlotSelectionWindow = new TimeSlotSelectionWindow {DataContext = _timeSlotSelectionViewModel};
            _timeSlotSelectionWindow.ShowDialog();
        }

        private void SetTestDataTimeSlotSelection()
        {
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
            var calendar = new CalendarViewModel(months)
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
            SetRequestedTimeSlot(date, _timeSlotSelectionViewModel.StartTimeSpan, _timeSlotSelectionViewModel.EndTimeSpan);
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
    }
}

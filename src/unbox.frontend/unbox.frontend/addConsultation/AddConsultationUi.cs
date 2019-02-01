using System;
using System.Collections.Generic;
using System.Linq;
using unbox.contracts;
using unbox.frontend.addConsultation.view;
using unbox.frontend.addConsultation.viewModels;
using unbox.frontend.enums;
using unbox.frontend.helper;
using unbox.frontend.viewmodels.timeslotcalendar;
using unbox.frontend.viewmodels.timeslotplan;
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

        internal AddConsultationUi(IBackendRequestHandler backendRequestHandler)
        {
            _backendRequestHandler = backendRequestHandler;
        }

        internal void ShowConsultationUi()
        {
            if (_viewModel == null)
            {
                _viewModel = new AddConsultationViewModel(OnAddConsultationRequest, OnHasToShowSelectionTimeSlots, OnCloseRequest);
                _viewModel.Patient = "1";
                SetTestDataAddConsultationViewModel();
            }

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
                new DayViewModel(DateTime.Today)
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
                            Workload = WorkloadEnum.Green
                        },
                        new HourViewModel(16)
                        {
                            IsPatientAvailable = false,
                            Workload = WorkloadEnum.Red
                        },
                        new HourViewModel(17)
                        {
                            IsPatientAvailable = true,
                            Workload = WorkloadEnum.Blocked
                        },
                        new HourViewModel(18)
                        {
                            IsPatientAvailable = false,
                            Workload = WorkloadEnum.Yellow
                        },
                    }
                },
                new DayViewModel(DateTime.Today.AddDays(1))
                {
                    Hours = new List<HourViewModel>
                    {
                        new HourViewModel(8)
                        {
                            IsPatientAvailable = false,
                            Workload = WorkloadEnum.Green
                        },
                        new HourViewModel(9)
                        {
                            IsPatientAvailable = false,
                            Workload = WorkloadEnum.Green
                        },
                        new HourViewModel(10)
                        {
                            IsPatientAvailable = false,
                            Workload = WorkloadEnum.Red
                        },
                        new HourViewModel(11)
                        {
                            IsPatientAvailable = true,
                            Workload = WorkloadEnum.Blocked
                        },
                        new HourViewModel(12)
                        {
                            IsPatientAvailable = false,
                            Workload = WorkloadEnum.Yellow
                        },
                    }
                }
            };
        }


        private void OnAddConsultationRequest()
        {
            _backendRequestHandler.Handle(DtoMapper.Map(_viewModel));
            _window.Close();
        }
        private void OnHasToShowSelectionTimeSlots()
        {
            _timeSlotSelectionViewModel = new TimeSlotSelectionViewModel(AddTimeSlot);
            SetTestDataTimeSlotSelection();
            _timeSlotSelectionWindow = new TimeSlotSelectionWindow {DataContext = _timeSlotSelectionViewModel};
            _timeSlotSelectionWindow.ShowDialog();
        }

        private void SetTestDataTimeSlotSelection()
        {
            var dummySlots = new List<TimeSlotViewModel>();
            dummySlots.Add(new TimeSlotViewModel(new DateTime(2019,2,2,8,0,0), new DateTime(2019,2,2,20,0,0), new DateTime(2019,2,2,8,0,0), new TimeSpan(6,0,0)));
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
            var calendar = new CalendarViewModel(months, dummySlots)
            {
                SelectedMonth = months.First()
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
            _viewModel.RequestedTimeSlotString = TimeSlotStringMapper.Map(_timeSlotSelectionViewModel.StartTimeSpan,
                                                _timeSlotSelectionViewModel.EndTimeSpan);
            if (_timeSlotSelectionViewModel.StartTimeSpan != null)
            {
                _viewModel.RequestedStart = (DateTime)(date + _timeSlotSelectionViewModel.StartTimeSpan);
            }

            if (_timeSlotSelectionViewModel.EndTimeSpan != null)
            {
                _viewModel.RequestedEnd = (DateTime)(date + _timeSlotSelectionViewModel.EndTimeSpan);
            }           
        }
    }
}

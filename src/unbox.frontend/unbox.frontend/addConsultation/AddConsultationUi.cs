using System;
using System.Collections.Generic;
using unbox.frontend.addConsultation.view;
using unbox.frontend.addConsultation.viewModels;
using unbox.frontend.enums;
using unbox.frontend.viewmodels.nexttimeslots;

namespace unbox.frontend.addConsultation
{
    internal class AddConsultationUi
    {
        private AddConsultationViewModel _viewModel;
        private AddConsultationWindow _window;

        private TimeSlotSelectionViewModel _timeSlotSelectionViewModel;
        private TimeSlotSelectionWindow _timeSlotSelectionWindow;

        internal void ShowConsultationUi()
        {
            if (_viewModel == null)
            {
                _viewModel = new AddConsultationViewModel(OnAddConsultationRequest, OnHasToShowSelectionTimeSlots, OnCloseRequest);
                GenerateTestData();
            }

            _window = new AddConsultationWindow();
            _window.DataContext = _viewModel;
            _window.Show();
        }

        private void OnCloseRequest()
        {
            _window.Close();
        }

        private void GenerateTestData()
        {
            _viewModel.Days = new List<DayViewModel>()
            {
                new DayViewModel("heute")
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
                new DayViewModel("morgen")
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
            throw new NotImplementedException();
        }
        private void OnHasToShowSelectionTimeSlots()
        {
            _timeSlotSelectionViewModel = new TimeSlotSelectionViewModel();
            _timeSlotSelectionWindow = new TimeSlotSelectionWindow {DataContext = _timeSlotSelectionViewModel};
            _timeSlotSelectionWindow.ShowDialog();
        }

    }
}

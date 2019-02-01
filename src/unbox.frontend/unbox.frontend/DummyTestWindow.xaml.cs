using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using unbox.frontend.enums;
using unbox.frontend.viewmodels.nexttimeslots;
using unbox.frontend.viewmodels.timeslotcalendar;
using unbox.frontend.viewmodels.timeslotplan;
using DayViewModel = unbox.frontend.viewmodels.nexttimeslots.DayViewModel;
using HourViewModel = unbox.frontend.viewmodels.nexttimeslots.HourViewModel;

namespace unbox.frontend
{
    /// <summary>
    /// Interaction logic for DummyTestWindow.xaml
    /// </summary>
    public partial class DummyTestWindow : Window
    {
        public DummyTestWindow()
        {
            InitializeComponent();
            nts.DataContext = this;

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
                SelectedMonth = months.First()
            };

            cal.DataContext = calendar;
            dat.DataContext = calendar;
            Days = new List<DayViewModel>()
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
                new DayViewModel("morgen", DateTime.Today.AddDays(1))
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

            TimeSlots = new List<TimeSlotViewModel>
            {
                new TimeSlotViewModel(new DateTime(2018,05,12,08,00,00), new DateTime(2018,05,12,12,00,00), new DateTime(2018,05,12,09,00,00), new TimeSpan(1,00,00), true),
                new TimeSlotViewModel(new DateTime(2018,05,12,07,00,00), new DateTime(2018,05,12,13,00,00), new DateTime(2018,05,12,10,00,00), new TimeSpan(1,30,00))
            };
            pln.DataContext = this;

        }

        public List<DayViewModel> Days { get; set; }
        public List<TimeSlotViewModel> TimeSlots { get; set; }
    }
}

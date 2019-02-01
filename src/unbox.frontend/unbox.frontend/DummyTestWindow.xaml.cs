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

            var dummySlots = new List<TimeSlotViewModel>();
            dummySlots.Add(new TimeSlotViewModel(new DateTime(2019,2,2,8,0,0), new DateTime(2019,2,2,20,0,0), new DateTime(2019,2,2,8,0,0), new TimeSpan(3,0,0)));

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

            cal.DataContext = calendar;
            dat.DataContext = calendar;
            tim1.DataContext = calendar;
            tim2.DataContext = calendar;
            nts.DataContext = new NextTimeSlotsViewModel(new List<DayViewModel>
            {
                new DayViewModel(DateTime.Today)
                {
                    Hours = new List<HourViewModel>
                    {
                        new HourViewModel(14)
                        {
                            IsPatientAvailable = false
                        },
                        new HourViewModel(15)
                        {
                            IsPatientAvailable = false
                        },
                        new HourViewModel(16)
                        {
                            IsPatientAvailable = false
                        },
                        new HourViewModel(17)
                        {
                            IsPatientAvailable = true
                        },
                        new HourViewModel(18)
                        {
                            IsPatientAvailable = false
                        },
                    }
                },
                new DayViewModel(DateTime.Today.AddDays(1))
                {
                    Hours = new List<HourViewModel>
                    {
                        new HourViewModel(8)
                        {
                            IsPatientAvailable = false
                        },
                        new HourViewModel(9)
                        {
                            IsPatientAvailable = false
                        },
                        new HourViewModel(10)
                        {
                            IsPatientAvailable = false
                        },
                        new HourViewModel(11)
                        {
                            IsPatientAvailable = true
                        },
                        new HourViewModel(12)
                        {
                            IsPatientAvailable = false
                        },
                    }
                }

            }, dummySlots);

            TimeSlots = new List<TimeSlotViewModel>
            {
                new TimeSlotViewModel(new DateTime(2018,05,12,08,00,00), new DateTime(2018,05,12,12,00,00), new DateTime(2018,05,12,09,00,00), new TimeSpan(1,00,00), true),
                new TimeSlotViewModel(new DateTime(2018,05,12,07,00,00), new DateTime(2018,05,12,13,00,00), new DateTime(2018,05,12,10,00,00), new TimeSpan(1,30,00))
            };
            pln.DataContext = this;

        }

        public List<TimeSlotViewModel> TimeSlots { get; set; }
    }
}

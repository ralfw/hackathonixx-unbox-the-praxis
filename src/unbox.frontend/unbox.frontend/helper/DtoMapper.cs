using System;
using System.Linq;
using unbox.contracts;
using unbox.frontend.addConsultation.viewModels;

namespace unbox.frontend.helper
{
    internal static class DtoMapper
    {
        public static RegisterConsultationCommand Map(AddConsultationViewModel viewModel)
        {
            var registerConsultationCommand = new RegisterConsultationCommand();
            registerConsultationCommand.PatientId = viewModel.Patient;
            registerConsultationCommand.RequestedTimeslot = new Timeslot();
            if (!viewModel.IsUrgent)
            {
                registerConsultationCommand.RequestedTimeslot.Start = viewModel.RequestedStart;
                registerConsultationCommand.RequestedTimeslot.End = viewModel.RequestedEnd;
            }
            else
            {
                foreach (var viewModelDay in viewModel.DataContextNextCalendar.Days)
                {
                    var firstChecked = viewModelDay.Hours.FirstOrDefault(h => h.IsPatientAvailable);
                    if (firstChecked != null)
                    {
                        registerConsultationCommand.RequestedTimeslot.Start =
                            TimeSlotStringMapper.Map(viewModelDay.Date, firstChecked.HourInt);
                        var lastChecked = firstChecked;
                        for(var i = viewModelDay.Hours.IndexOf(firstChecked); i < viewModelDay.Hours.Count; i++)
                        {
                            if (viewModelDay.Hours[i].IsPatientAvailable)
                            {
                                lastChecked = viewModelDay.Hours[i];
                            }
                            else
                            {
                                break;
                            }
                        }
                        registerConsultationCommand.RequestedTimeslot.End = TimeSlotStringMapper.Map(viewModelDay.Date, lastChecked.HourInt+1);

                    }
                }
            }
            
            registerConsultationCommand.RequestedTimeslot.Duration = new TimeSpan(0,0, viewModel.RequestedDuration,0);
            return registerConsultationCommand;
        }
    }
}

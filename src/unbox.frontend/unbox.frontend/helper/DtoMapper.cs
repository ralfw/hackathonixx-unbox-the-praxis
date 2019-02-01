using System;
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
            registerConsultationCommand.RequestedTimeslot.Start = viewModel.RequestedStart;
            registerConsultationCommand.RequestedTimeslot.End = viewModel.RequestedEnd;
            registerConsultationCommand.RequestedTimeslot.Duration = new TimeSpan(0,0, viewModel.RequestedDuration,0);
            return registerConsultationCommand;
        }
    }
}

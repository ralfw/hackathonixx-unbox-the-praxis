using System;
using System.Collections.Generic;

namespace unbox.contracts
{
    public class CurrentPlanResult {
        public IEnumerable<PlannedConsultation> Schedule;

        public class PlannedConsultation {
            public string ConsultationId;
            public string PatientId;
            public Timeslot RequestedTimeslot;
            public DateTime AssignedTimeslotStart;
        }
    }
}
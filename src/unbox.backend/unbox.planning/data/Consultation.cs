using System;
using unbox.contracts;

namespace unbox.planning.data
{
    public class Consultation
    {
        public string ConsultationId;
        public string PatientId;
        public Timeslot RequestedTimeslot;

        public DateTime? PlannedStart;

        public Timeslot ActualTimeslot;
        public bool Fixed;
    }
}
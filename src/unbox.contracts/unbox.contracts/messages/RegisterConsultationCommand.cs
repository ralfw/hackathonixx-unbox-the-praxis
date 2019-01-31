namespace unbox.contracts
{
    public class RegisterConsultationCommand {
        public string ConsultationId;
        public string PatientId;
        public Timeslot RequestedTimeslot;
    }
}
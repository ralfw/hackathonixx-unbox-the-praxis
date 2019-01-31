using System;

namespace unbox.contracts
{
    public class RegisterRealTimeslotCommand {
        public string ConsultationId;
        public DateTime RealTimeslotStart;
        public TimeSpan RealDuration;
    }
}
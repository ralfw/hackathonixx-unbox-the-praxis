using System;
using System.Collections.Generic;

namespace unbox.contracts
{
    public class ConfigureCommand {
        public DateTime OfficeHoursStart;
        public DateTime OfficeHoursEnd;
        public List<Timeslot> Breaks;
    }
}
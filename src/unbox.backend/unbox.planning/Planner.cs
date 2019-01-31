using System;
using System.Collections.Generic;
using unbox.planning.data;

namespace unbox.planning
{
    public class Planner
    {
        public static bool UpdatePlan(List<CalendarEntry> calendar, List<Consultation> consultations)
        {
            foreach (var consultation in consultations)
            {
                if (consultation.PlannedStart == null)
                {
                }
               
            }  

            return false;
        }
    }
}
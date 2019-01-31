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
                    var newConsultation = new CalendarEntry();
                    newConsultation.Start = consultation.RequestedTimeslot.Start;
                    
                    newConsultation.End = consultation.RequestedTimeslot.Start.Add(consultation.RequestedTimeslot.Duration);
                    if (CheckCollition())
                    {

                    }
                    else
                    {
                        consultation.PlannedStart = consultation.RequestedTimeslot.Start;
                        calendar.Add(new CalendarEntry(){
                            Start = consultation.RequestedTimeslot.Start,
                            
                        });
                        return true;
                    }
                }
               
            }  

            return false;
        }

        private static bool CheckCollition()
        {
            throw new NotImplementedException();
        }
    }
}
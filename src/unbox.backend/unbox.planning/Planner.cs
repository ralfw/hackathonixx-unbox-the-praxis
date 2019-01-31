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
                    var temporaryEnd = consultation.RequestedTimeslot.Start.Add(consultation.RequestedTimeslot.Duration);
                    newConsultation.ConsultationId = consultation.ConsultationId;
                    newConsultation.End = temporaryEnd;
                    if (HasCollition(calendar, newConsultation))
                    {
                        return false;
                    }
                    else
                    {
                        consultation.PlannedStart = consultation.RequestedTimeslot.Start;
                        calendar.Add(newConsultation);
                        return true;
                    }
                }
               
            }  

            return true;
        }

        private static bool HasCollition(List<CalendarEntry> calendar, CalendarEntry newConsultation)
        {
            foreach(var element in calendar)
            {
                if ((newConsultation.Start <= element.Start && newConsultation.End >= element.Start)||(newConsultation.Start >= element.Start && element.End > newConsultation.Start))
                {
                    return true;
                }
            }
            return false; 
        }
    }
}
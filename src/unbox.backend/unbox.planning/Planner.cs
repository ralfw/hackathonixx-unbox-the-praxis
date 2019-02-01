using System;
using System.Collections.Generic;
using System.Linq;
using unbox.planning.data;

namespace unbox.planning
{
    public class Planner
    {
        public static bool UpdatePlan(List<CalendarEntry> calendar, List<Consultation> consultations)
        {
            var unableToPlan = new List<Consultation>();
            
            foreach (var consultation in consultations)
            {
                if (consultation.PlannedStart.HasValue) continue;

                var tentativeCalendarEntry = new CalendarEntry {
                    ConsultationId = consultation.ConsultationId,
                    Start = consultation.RequestedTimeslot.Start
                };
                var temporaryEnd = consultation.RequestedTimeslot.Start.Add(consultation.RequestedTimeslot.Duration);
                tentativeCalendarEntry.End = temporaryEnd;

                
                if (HasCollision(calendar, tentativeCalendarEntry)) {
                    var newPlannedTime = FindNewConsultationOption(calendar, consultation, tentativeCalendarEntry);
                    if( newPlannedTime.HasValue) {
                        tentativeCalendarEntry.Start = newPlannedTime.Value;
                        consultation.PlannedStart = tentativeCalendarEntry.Start;
                        calendar.Add(tentativeCalendarEntry);
                    }
                    else {
                        // hier neuen teil einfügen
                        
                        unableToPlan.Add(consultation);
                    }
                }
                else {
                    consultation.PlannedStart = tentativeCalendarEntry.Start;
                    calendar.Add(tentativeCalendarEntry);
                }
            }  

            unableToPlan.ForEach(c => consultations.Remove(c));

            return unableToPlan.Count == 0;
        }

        
        private static DateTime? FindNewConsultationOption(List<CalendarEntry> calendar, Consultation consultation, CalendarEntry tentativeCalendarEntry)
        {
            do {
                tentativeCalendarEntry.Start = tentativeCalendarEntry.Start.AddMinutes(5);
                if (HasCollision(calendar, tentativeCalendarEntry) is false) {
                    return tentativeCalendarEntry.Start;
                }
            }while((tentativeCalendarEntry.Start.Add(consultation.RequestedTimeslot.Duration) < consultation.RequestedTimeslot.End));

            return null;
        }

        private static bool HasCollision(List<CalendarEntry> calendar, CalendarEntry newConsultation) {
            foreach(var element in calendar) {

                if ((newConsultation.Start <= element.Start && newConsultation.End >= element.Start) ||
                    (newConsultation.Start >= element.Start && element.End > newConsultation.Start)) {
                    return true;
                }
            }
            return false; 
        }
    }
}
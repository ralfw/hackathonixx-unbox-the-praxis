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

                
                if (HasCollision(calendar, tentativeCalendarEntry) !=  new TimeSpan(0,0,0)) {
                    var newPlannedTime = FindNewConsultationOption(calendar, consultation, tentativeCalendarEntry);
                    if( newPlannedTime.HasValue) {
                        tentativeCalendarEntry.Start = newPlannedTime.Value;

                        consultation.PlannedStart = tentativeCalendarEntry.Start;
                        calendar.Add(tentativeCalendarEntry);
                    }
                    else {
                        unableToPlan.Add(consultation);
                    }
                }
                else {
                    consultation.PlannedStart = tentativeCalendarEntry.Start;
                    calendar.Add(tentativeCalendarEntry);
                }
            }  

            unableToPlan.ForEach(c => consultations.Remove(c));

            //TODO: Verbessern: Planner liefert derzeit false, auch wenn von 3 unplanned consultations 2 eingeplant werden konnten
            return unableToPlan.Count == 0;
        }

        
        private static DateTime? FindNewConsultationOption(List<CalendarEntry> calendar, Consultation consultation, CalendarEntry tentativeCalendarEntry)
        {
            do {
                tentativeCalendarEntry.Start = tentativeCalendarEntry.Start.AddMinutes(5);
                if (HasCollision(calendar, tentativeCalendarEntry) == new TimeSpan(0,0,0)) {
                    return tentativeCalendarEntry.Start;
                }
            }while((tentativeCalendarEntry.Start.Add(consultation.RequestedTimeslot.Duration) < consultation.RequestedTimeslot.End));

            return null;
        }

        private static TimeSpan? HasCollision(List<CalendarEntry> calendar, CalendarEntry newConsultation) {

            

            foreach(var element in calendar) {
               
                TimeSpan nextCollidation = CollidateWithNext(element, newConsultation);
                TimeSpan previousCollidation = CollidateWithPrevious(element, newConsultation);
               
                var sumCollidation = previousCollidation + nextCollidation;
                if(sumCollidation != new TimeSpan(0, 0, 0))
                {
                    return sumCollidation;
                }
                

                //if ((newConsultation.Start <= element.Start && newConsultation.End >= element.Start) ||
                //    (newConsultation.Start >= element.Start && element.End > newConsultation.Start)) {
                //    return true;
                //}
                
            }
            return new TimeSpan(0,0,0); 
        }

        private static TimeSpan CollidateWithNext(CalendarEntry element, CalendarEntry newConsultation)
        {
            if (newConsultation.Start < element.Start && newConsultation.End > element.Start)
            {
                return (newConsultation.End - element.Start);
            }
            else
            {
                return new TimeSpan(0,0,0);
            }
        }

        private static TimeSpan CollidateWithPrevious(CalendarEntry element, CalendarEntry newConsultation)
        {
            if(newConsultation.Start >= element.Start && element.End > newConsultation.Start)
            {
                return (element.End - newConsultation.Start);
            }
            else
            {
                return new TimeSpan(0, 0, 0);
            }
        }
    }
}
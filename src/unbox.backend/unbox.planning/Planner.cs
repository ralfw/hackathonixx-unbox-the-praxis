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

                var collision = HasCollision(calendar, tentativeCalendarEntry);
                if (collision.Sum !=  new TimeSpan(0,0,0)) {
                    var newPlannedTime = FindNewConsultationOption(calendar, consultation, tentativeCalendarEntry);
                    if( newPlannedTime.HasValue) {
                        tentativeCalendarEntry.Start = newPlannedTime.Value;

                        consultation.PlannedStart = tentativeCalendarEntry.Start;
                        calendar.Add(tentativeCalendarEntry);
                    }
                    else {
                        var collidingConsultation = consultations.First(x => x.ConsultationId.Equals(collision.Next));


                        collidingConsultation.PlannedStart = collidingConsultation.PlannedStart.Value.AddMinutes(5);
                        calendar.First(x => x.ConsultationId.Equals(collision.Next)).Start.Add(collision.NextCollition);
                        calendar.First(x => x.ConsultationId.Equals(collision.Next)).End.Add(collision.NextCollition);

                        consultation.PlannedStart = tentativeCalendarEntry.Start;
                        calendar.Add(tentativeCalendarEntry);



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
                var collision = HasCollision(calendar, tentativeCalendarEntry);
                if (collision.Sum == new TimeSpan(0,0,0)) {
                    return tentativeCalendarEntry.Start;
                }
            }while((tentativeCalendarEntry.Start.Add(consultation.RequestedTimeslot.Duration) < consultation.RequestedTimeslot.End));

            return null;
        }


        private static Collition HasCollision(List<CalendarEntry> calendar, CalendarEntry newConsultation) {
            Collition result = new Collition();
            result.Sum = new TimeSpan(0, 0, 0);
            

            foreach(var element in calendar) {
               
                TimeSpan nextCollidation = CollidateWithNext(element, newConsultation);
                TimeSpan previousCollidation = CollidateWithPrevious(element, newConsultation);
               
                var sumCollidation = previousCollidation + nextCollidation;
                if(sumCollidation != new TimeSpan(0, 0, 0))
                {
                    result.Sum = sumCollidation;
                    result.NextCollition = nextCollidation;
                    result.PreviousCollition = previousCollidation;
                    if(result.NextCollition != new TimeSpan(0, 0, 0))
                    {
                        result.Next = element.ConsultationId;
                    }
                    if (result.PreviousCollition != new TimeSpan(0, 0, 0))
                    {
                        result.Previous = element.ConsultationId;
                    }

                    return result;
                }
                

                //if ((newConsultation.Start <= element.Start && newConsultation.End >= element.Start) ||
                //    (newConsultation.Start >= element.Start && element.End > newConsultation.Start)) {
                //    return true;
                //}
                
            }
            return result ; 
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

        private class Collition
        {
            public TimeSpan Sum;
            public TimeSpan PreviousCollition;
            public TimeSpan NextCollition;

            public string Previous;
            public string Next;

        }
    }
}
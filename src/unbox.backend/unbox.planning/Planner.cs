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
                    var ConsultationOption = new CalendarEntry();
                    ConsultationOption.Start = consultation.RequestedTimeslot.Start;
                    var temporaryEnd = consultation.RequestedTimeslot.Start.Add(consultation.RequestedTimeslot.Duration);
                    ConsultationOption.ConsultationId = consultation.ConsultationId;
                    ConsultationOption.End = temporaryEnd;
                    if (HasCollision(calendar, ConsultationOption))
                    {
                        var newPlannedTime = FindNewConsultationOption(calendar, consultation, ConsultationOption);
                        if( newPlannedTime == null)
                        {
                            consultations.Remove(consultation);
                            return false;
                        }
                        else
                        {
                            consultation.PlannedStart = newPlannedTime;
                            calendar.Add(ConsultationOption);
                            return true;
                        }
                    }
                    else
                    {
                        consultation.PlannedStart = consultation.RequestedTimeslot.Start;
                        calendar.Add(ConsultationOption);
                        return true;
                    }
                }
               
            }  

            return true;
        }

        private static DateTime? FindNewConsultationOption(List<CalendarEntry> calendar, Consultation consultation, CalendarEntry consultationOption)
        {
            bool consultationSet = false;
            do
            {
                consultationOption.Start.AddMinutes(5);
                if(!HasCollision(calendar, consultationOption))
                {
                    consultationSet = true;
                    return consultationOption.Start;
                }
            }while (!consultationSet && (consultationOption.Start.Add(consultation.RequestedTimeslot.Duration) < consultation.RequestedTimeslot.End));

            return null;

        }

        private static bool HasCollision(List<CalendarEntry> calendar, CalendarEntry newConsultation)
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
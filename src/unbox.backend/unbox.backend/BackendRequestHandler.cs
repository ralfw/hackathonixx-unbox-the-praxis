using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unbox.contracts;
using unbox.planning;
using unbox.planning.data;

namespace unbox.backend
{
    public class BackendRequestHandler : IBackendRequestHandler
    {
        private readonly List<Consultation> _consultations = new List<Consultation>();
        
        
        public void Handle(ConfigureCommand cmd)
        {
            throw new NotImplementedException();
        }

        public bool Handle(RegisterConsultationCommand cmd) {
            Register(cmd);
            var cal = Build_calendar();
            var plan = Planner.UpdatePlan(cal, _consultations);
            return plan;
        }

        public void Handle(RegisterRealTimeslotCommand cmd) {
            var consultation = _consultations.FirstOrDefault(cons => cons.ConsultationId == cmd.ConsultationId);
            
            if (consultation != null) {
                consultation.ActualTimeslot = new Timeslot {
                    Start = cmd.RealTimeslotStart,
                    Duration = cmd.RealDuration,
                    End = cmd.RealTimeslotStart.Add(cmd.RealDuration)
                };
            }
        }

        public CurrentPlanResult Handle(CurrentPlanQuery query)
        {
            var result = _consultations.Where(cons => cons.PlannedStart.HasValue && 
                                                      cons.PlannedStart.Value.Year == query.Date.Year &&
                                                      cons.PlannedStart.Value.Month == query.Date.Month &&
                                                      cons.PlannedStart.Value.Day == query.Date.Day)
                                       .OrderBy(cons => cons.PlannedStart);
            
            return new CurrentPlanResult {
                Schedule = result.Select(cons => new CurrentPlanResult.PlannedConsultation {
                                                            ConsultationId = cons.ConsultationId,
                                                            PatientId = cons.PatientId,
                                                            AssignedTimeslotStart = cons.PlannedStart.Value,
                                                            RequestedTimeslot = cons.RequestedTimeslot
                                                        })
            };
        }
        
        
        public void HandleNotificationRequest() => HandleNotificationRequest(DateTime.Now);
        public int HandleNotificationRequest(DateTime refTime) {
            var toNotify = _consultations.Where(cons => !cons.HasBeenNotified &&
                                                        cons.PlannedStart.HasValue &&
                                                        refTime >= cons.PlannedStart.Value.Subtract(cons.NotificationLeadTime))
                                         .ToArray();
            
            var notProv = new PushoverNotificationProvider();
            foreach (var cons in toNotify) {
                notProv.Send("Ihr Termin wurde geplant!", 
                            $"Lieber {cons.PatientId}, bitte kommen Sie für {cons.PlannedStart:hh:mm dd.MM.yyyy} in die Praxis!", 
                             "http://medatixx.de");
                cons.HasBeenNotified = true;
            }

            return toNotify.Length;
        }


        void Register(RegisterConsultationCommand cmd) {
            _consultations.Add(new Consultation {
                ConsultationId = cmd.ConsultationId,
                PatientId = cmd.PatientId,
                RequestedTimeslot = cmd.RequestedTimeslot,
                NotificationLeadTime = new TimeSpan(0,1,0,0),
                HasBeenNotified = false,
                PlannedStart = null,
                ActualTimeslot = null,
                Fixed = false
            });   
        }

        
        List<CalendarEntry> Build_calendar() {
            var cal = new List<CalendarEntry>();

            if (_consultations.Any()) {
                var d = _consultations.First().RequestedTimeslot.Start;
                cal.Add(new CalendarEntry {
                    ConsultationId = "Zu früh",
                    Start = new DateTime(d.Year, d.Month, d.Day, 0, 0, 0),
                    End = new DateTime(d.Year, d.Month, d.Day, 7, 59, 0)
                });
                cal.Add(new CalendarEntry {
                    ConsultationId = "Mahlzeit",
                    Start = new DateTime(d.Year, d.Month, d.Day, 12, 0, 0),
                    End = new DateTime(d.Year, d.Month, d.Day, 13, 0, 0)
                });
                cal.Add(new CalendarEntry {
                    ConsultationId = "Zu spät",
                    Start = new DateTime(d.Year, d.Month, d.Day, 18, 1, 0),
                    End = new DateTime(d.Year, d.Month, d.Day, 23, 59, 0)
                });
            }

            cal.AddRange(_consultations.Where(cons => cons.ActualTimeslot != null)
                                       .Select(cons => new CalendarEntry {
                                            ConsultationId = cons.ConsultationId,
                                            Start = cons.ActualTimeslot.Start,
                                            End = cons.ActualTimeslot.End
                                       }));
            cal.AddRange(_consultations.Where(cons => cons.PlannedStart.HasValue)
                                       .Select(cons => new CalendarEntry {
                                            ConsultationId = cons.ConsultationId,
                                            Start = cons.PlannedStart.Value,
                                            End = cons.PlannedStart.Value.Add(cons.RequestedTimeslot.Duration)
                                       }));
            return cal;
        }
    }
}

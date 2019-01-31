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
            return Planner.UpdatePlan(cal, _consultations);
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

        public CurrentPlanResult Handle(CurrentPlanQuery query) {
            var result = _consultations.Where(cons => cons.PlannedStart.HasValue && 
                                                      cons.PlannedStart.Value.Date == query.Date.Date)
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


        void Register(RegisterConsultationCommand cmd)
        {
            _consultations.Add(new Consultation {
                ConsultationId = cmd.ConsultationId,
                PatientId = cmd.PatientId,
                RequestedTimeslot = cmd.RequestedTimeslot,
                PlannedStart = DateTime.MinValue,
                ActualTimeslot = null,
                Fixed = false
            });   
        }

        
        List<CalendarEntry> Build_calendar() {
            var cal = new List<CalendarEntry>();
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

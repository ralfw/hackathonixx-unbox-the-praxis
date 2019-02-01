using System;
using System.Globalization;
using System.IO;
using System.Linq;
using NUnit.Framework;
using unbox.contracts;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Threading;
using unbox.planning.data;

namespace unbox.planning.tests
{
    [TestFixture]
    public class BruteForcePlanning
    {
        [SetUp]
        public void Setup() => Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
        
        
        [Test]
        public void Test1() {
            var consultations = Import("consultation request samples.csv");
            /*
            var consultations = new List<Consultation>();
            consultations.Add(new Consultation
            {
                ConsultationId = "1",
                PatientId = "Balin",
                RequestedTimeslot = new Timeslot
                {
                    Start = new DateTime(2019,2,4, 8,0,0),
                    End = new DateTime(2019,2,4, 10,0,0),
                    Duration = new TimeSpan(0,0,30,0)
                }
            });
            consultations.Add(new Consultation
            {
                ConsultationId = "2",
                PatientId = "Kili",
                RequestedTimeslot = new Timeslot
                {
                    Start = new DateTime(2019,2,4, 8,15,0),
                    End = new DateTime(2019,2,4, 9,0,0),
                    Duration = new TimeSpan(0,0,30,0)
                }
            });
            consultations.Add(new Consultation
            {
                ConsultationId = "3",
                PatientId = "Oin",
                RequestedTimeslot = new Timeslot
                {
                    Start = new DateTime(2019,2,4, 10,0,0),
                    End = new DateTime(2019,2,4, 11,0,0),
                    Duration = new TimeSpan(0,0,15,0)
                }
            });
*/

            var i = 0;
            var plans = new List<IEnumerable<PlanEntry>>();
            Do_brute_force_planning(new Stack<PlanEntry>(), consultations, 
                                    plan =>
                                    {
                                        if (i++ % 1000 == 0) Debug.WriteLine($"<<<{i}>>>");
                                        plans.Add(new List<PlanEntry>(plan));
                                    });

            Debug.WriteLine($"number of plans: {plans.Count}");
            return;
            
            foreach (var plan in plans) {
                Debug.Print("---");
                foreach(var entry in plan.OrderBy(e => e.PlannedTimeslot.Start))
                    Debug.WriteLine($"{entry.Consultation.PatientId}: {entry.PlannedTimeslot.Start}..{entry.PlannedTimeslot.End}");
            }
        }

        
        //TODO: fixed consultations nicht verschieben
        void Do_brute_force_planning(Stack<PlanEntry> plan, IEnumerable<Consultation> consultations,
                                     Action<IEnumerable<PlanEntry>> onPlanFound) {
            if (consultations.Any() is false) {
                onPlanFound(plan);
                return;
            }

            foreach (var cons in consultations) {
                //Debug.WriteLine($"{"".PadRight(2*plan.Count)}{cons.PatientId}");
                if (Try_to_place_in_plan(plan, cons)) {
                    Do_brute_force_planning(plan, consultations.Where(x => x.ConsultationId != cons.ConsultationId), 
                                            onPlanFound);
                    plan.Pop();
                }
            }
        }

        bool Try_to_place_in_plan(Stack<PlanEntry> plan, Consultation consultation) {
            var gaps = Calculate_gaps(plan);

            if (gaps.Length == 0) {
                var entry = new PlanEntry {
                    Consultation = consultation,
                    PlannedTimeslot = new Timeslot {
                        Start = consultation.RequestedTimeslot.Start,
                        End = consultation.RequestedTimeslot.Start.Add(consultation.RequestedTimeslot.Duration),
                        Duration = consultation.RequestedTimeslot.Duration
                    }
                };
                plan.Push(entry);
                return true;
            }

            foreach (var gap in gaps) {
                // gap inside requested slot
                if (gap.Start >= consultation.RequestedTimeslot.Start &&
                    gap.End <= consultation.RequestedTimeslot.End) {
                    var entry = new PlanEntry {
                        Consultation = consultation,
                        PlannedTimeslot = new Timeslot {
                            Start = gap.Start,
                            End = gap.Start.Add(consultation.RequestedTimeslot.Duration),
                            Duration = consultation.RequestedTimeslot.Duration
                        }
                    };
                    plan.Push(entry);
                    return true;
                }
                
                // gap encompasses requested slot
                if (gap.Start <= consultation.RequestedTimeslot.Start &&
                    gap.End >= consultation.RequestedTimeslot.End) {
                    var entry = new PlanEntry {
                        Consultation = consultation,
                        PlannedTimeslot = new Timeslot {
                            Start = consultation.RequestedTimeslot.Start,
                            End = consultation.RequestedTimeslot.Start.Add(consultation.RequestedTimeslot.Duration),
                            Duration = consultation.RequestedTimeslot.Duration
                        }
                    };
                    plan.Push(entry);
                    return true;
                }
                
                // gap overlaps at beginning of slot
                if (gap.Start < consultation.RequestedTimeslot.Start &&
                    gap.End > consultation.RequestedTimeslot.Start &&
                    gap.End <= consultation.RequestedTimeslot.End &&
                    gap.End.Subtract(consultation.RequestedTimeslot.Start) >= consultation.RequestedTimeslot.Duration) {
                    var entry = new PlanEntry {
                        Consultation = consultation,
                        PlannedTimeslot = new Timeslot {
                            Start = consultation.RequestedTimeslot.Start,
                            End = consultation.RequestedTimeslot.Start.Add(consultation.RequestedTimeslot.Duration),
                            Duration = consultation.RequestedTimeslot.Duration
                        }
                    };
                    plan.Push(entry);
                    return true;
                }
                
                // gap overlaps at the end of slot
                if (gap.Start >= consultation.RequestedTimeslot.Start &&
                    gap.Start < consultation.RequestedTimeslot.End &&
                    gap.End > consultation.RequestedTimeslot.End &&
                    consultation.RequestedTimeslot.End.Subtract(gap.Start) >= consultation.RequestedTimeslot.Duration) {
                    var entry = new PlanEntry {
                        Consultation = consultation,
                        PlannedTimeslot = new Timeslot {
                            Start = gap.Start,
                            End = gap.Start.Add(consultation.RequestedTimeslot.Duration),
                            Duration = consultation.RequestedTimeslot.Duration
                        }
                    };
                    plan.Push(entry);
                    return true;
                }
            }
            
            return false;
        }
        
        Timeslot[] Calculate_gaps(Stack<PlanEntry> plan) {
            var sortedPlan = plan.OrderBy(e => e.PlannedTimeslot.Start).ToArray();
            if (sortedPlan.Length == 0) return new Timeslot[0];
            
            var gaps = new List<Timeslot>();
            var gap = new Timeslot {
                Start = new DateTime(sortedPlan.First().PlannedTimeslot.Start.Year, 
                                     sortedPlan.First().PlannedTimeslot.Start.Month,
                                     sortedPlan.First().PlannedTimeslot.Start.Day,
                                     8,0,0),
                End = new DateTime(sortedPlan.First().PlannedTimeslot.Start.Year, 
                                   sortedPlan.First().PlannedTimeslot.Start.Month,
                                   sortedPlan.First().PlannedTimeslot.Start.Day,
                                   sortedPlan.First().PlannedTimeslot.Start.Hour,
                                   sortedPlan.First().PlannedTimeslot.Start.Minute,
                                   sortedPlan.First().PlannedTimeslot.Start.Second)
            };
            gaps.Add(gap);
            
            
            for (var i = 1; i < sortedPlan.Length; i++) {
                gap = new Timeslot {
                    Start = new DateTime(sortedPlan[i-1].PlannedTimeslot.End.Year, 
                                        sortedPlan[i-1].PlannedTimeslot.End.Month,
                                        sortedPlan[i-1].PlannedTimeslot.End.Day,
                                        sortedPlan[i-1].PlannedTimeslot.End.Hour,
                                        sortedPlan[i-1].PlannedTimeslot.End.Minute,
                                        sortedPlan[i-1].PlannedTimeslot.End.Second),
                    End = new DateTime(sortedPlan[i].PlannedTimeslot.Start.Year, 
                                        sortedPlan[i].PlannedTimeslot.Start.Month,
                                        sortedPlan[i].PlannedTimeslot.Start.Day,
                                        sortedPlan[i].PlannedTimeslot.Start.Hour,
                                        sortedPlan[i].PlannedTimeslot.Start.Minute,
                                        sortedPlan[i].PlannedTimeslot.Start.Second)
                };
                gaps.Add(gap);
            }
            
            
            gap = new Timeslot {
                Start = new DateTime(sortedPlan.Last().PlannedTimeslot.End.Year, 
                    sortedPlan.Last().PlannedTimeslot.End.Month,
                    sortedPlan.Last().PlannedTimeslot.End.Day,
                    sortedPlan.Last().PlannedTimeslot.End.Hour,
                    sortedPlan.Last().PlannedTimeslot.End.Minute,
                    sortedPlan.Last().PlannedTimeslot.End.Second),
                End = new DateTime(sortedPlan.Last().PlannedTimeslot.End.Year, 
                    sortedPlan.Last().PlannedTimeslot.End.Month,
                    sortedPlan.Last().PlannedTimeslot.End.Day,
                    18,0,0),
            };
            gaps.Add(gap);

            return gaps.ToArray();
        }


        
        class PlanEntry {
            public Consultation Consultation;
            public Timeslot PlannedTimeslot;
        }

        IEnumerable<Consultation> Import(string filename) {
            var lines = File.ReadAllLines(filename);
            var records = lines.Skip(1).Select(line => line.Split(';'));
                
            var ci = new CultureInfo("de-DE");
            var formats = new[] {"dd.MM.yyyy"}.Union(ci.DateTimeFormat.GetAllDateTimePatterns()).ToArray();

            return records.Select(rec => new Consultation {
                ConsultationId = rec[0],
                PatientId = rec[1],
                RequestedTimeslot = new Timeslot {
                    Start = DateTime.ParseExact(rec[2], formats, ci, DateTimeStyles.AssumeLocal),
                    End = DateTime.ParseExact(rec[3], formats, ci, DateTimeStyles.AssumeLocal),
                    Duration = new TimeSpan(0,0,int.Parse(rec[4]),0)
                }
            });
        }
    }
}
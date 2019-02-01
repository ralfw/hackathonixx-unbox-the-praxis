using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using unbox.contracts;

namespace unbox.console.client
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var cmds = new List<RegisterConsultationCommand>();
            if (args.Length > 0) {
                var csvfilename = args[0];
                var lines = File.ReadAllLines(csvfilename);
                var records = lines.Skip(1).Select(line => line.Split(';'));
                
                var ci = new CultureInfo("de-DE");
                var formats = new[] {"dd.MM.yyyy"}.Union(ci.DateTimeFormat.GetAllDateTimePatterns()).ToArray();
                
                cmds.AddRange(records.Select(rec => new RegisterConsultationCommand {
                    ConsultationId = rec[0],
                    PatientId = rec[1],
                    RequestedTimeslot = new Timeslot {
                        
                        Start = DateTime.ParseExact(rec[2], formats, ci, DateTimeStyles.AssumeLocal),
                        End = DateTime.ParseExact(rec[3], formats, ci, DateTimeStyles.AssumeLocal),
                        Duration = new TimeSpan(0,0,int.Parse(rec[4]),0)
                    }
                }));
            }
            else
            {
                throw new NotImplementedException("CSV-Filename required as first param on command line!");
            }

            
            var reqh = new backend.BackendRequestHandler();
            
            foreach (var cmd in cmds) {
                Console.WriteLine();
                Console.WriteLine($"Next: {cmd.PatientId}: {cmd.RequestedTimeslot.Duration} between {cmd.RequestedTimeslot.Start:s} and {cmd.RequestedTimeslot.End:s}");
                Console.Write("To reg press ENTER");
                Console.ReadLine();

                var success = reqh.Handle(cmd);
                Console.WriteLine("  {0}", success ? "+1" : ":-(");

                if (success) {
                    var plan = reqh.Handle(new CurrentPlanQuery {Date = new DateTime(2019, 2, 4)});
                    foreach(var entry in plan.Schedule)
                        Console.WriteLine($"  {entry.PatientId}: {entry.AssignedTimeslotStart:s}, {entry.RequestedTimeslot.Duration}");
                }
            }
            
            /*
            Console.Write("To notify press ENTER");
            System.Environment.SetEnvironmentVariable("PUSHOVER_APP_TOKEN", "???");
            System.Environment.SetEnvironmentVariable("PUSHOVER_USER_KEY", "???");
            var n = reqh.HandleNotificationRequest(new DateTime(2019, 2, 4, 10,55,0));
            Console.WriteLine($"{n} patients notified!");
            */
        }
    }
}
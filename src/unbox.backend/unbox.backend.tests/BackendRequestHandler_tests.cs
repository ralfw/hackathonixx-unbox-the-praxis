using System;
using NUnit.Framework;
using unbox.contracts;

namespace unbox.backend.tests
{
    [TestFixture]
    public class BackendRequestHandler_tests
    {
        [Test, Explicit]
        public void Send_notifications()
        {
            System.Environment.SetEnvironmentVariable("PUSHOVER_APP_TOKEN", "ad9p542uaubfxmc3mcw4t4tu72or4n");
            System.Environment.SetEnvironmentVariable("PUSHOVER_USER_KEY", "2xvMY59qr2pnu3U1bGeHTAZnX8Zj5R");
            
            var sut = new BackendRequestHandler();
            sut.Handle(new RegisterConsultationCommand {
                ConsultationId = "1",
                PatientId = "Balin",
                RequestedTimeslot = new Timeslot {
                    Start = new DateTime(2019,2,4, 10,0,0),
                    End = new DateTime(2019,2,4, 12,0,0),
                    Duration = new TimeSpan(0,0,30,0),
                }
            });
            
            var result = sut.HandleNotificationRequest(new DateTime(2019,2,4, 9,5,0));
            
            Assert.AreEqual(1, result);
        }
    }
}
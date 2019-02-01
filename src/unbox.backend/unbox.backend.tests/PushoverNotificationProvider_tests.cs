using System;
using System.Globalization;
using NUnit.Framework;

namespace unbox.backend.tests
{
    [TestFixture]
    public class PushoverNotificationProvider_tests
    {
        [Test,Explicit]
        public void Send()
        {
            System.Environment.SetEnvironmentVariable("PUSHOVER_APP_TOKEN", "???");
            System.Environment.SetEnvironmentVariable("PUSHOVER_USER_KEY", "???");
            
            var sut = new PushoverNotificationProvider();
            sut.Send("my title", "my body: " + DateTime.Now, "http://medatixx.de");
        }
    }
}
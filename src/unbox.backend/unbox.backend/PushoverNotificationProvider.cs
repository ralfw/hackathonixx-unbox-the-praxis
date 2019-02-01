namespace unbox.backend
{
    public class PushoverNotificationProvider
    {
        public void Send(string title, string body)
        {
            var appToken = System.Environment.GetEnvironmentVariable("PUSHOVER_APP_TOKEN");
            var userKey = System.Environment.GetEnvironmentVariable("PUSHOVER_USER_KEY");
            
            
        }
    }
}
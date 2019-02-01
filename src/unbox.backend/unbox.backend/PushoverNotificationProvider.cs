using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;

namespace unbox.backend
{
    internal class PushoverNotificationProvider
    {
        public void Send(string title, string body, string url)
        {
            var appToken = System.Environment.GetEnvironmentVariable("PUSHOVER_APP_TOKEN");
            if (string.IsNullOrEmpty(appToken)) throw new InvalidOperationException("Missing environment variable PUSHOVER_APP_TOKEN!");

            var userKey = System.Environment.GetEnvironmentVariable("PUSHOVER_USER_KEY");
            if (string.IsNullOrEmpty(userKey)) throw new InvalidOperationException("Missing environment variable PUSHOVER_USER_KEY!");

            var json = @"{
    'token':'$PUSHOVER_APP_TOKEN',
    'user':'$PUSHOVER_USER_KEY',
    'message':'$MESSAGE',
    'title':'$TITLE',
    'url':'$URL'
}"
                .Replace("$PUSHOVER_APP_TOKEN", appToken)
                .Replace("$PUSHOVER_USER_KEY", userKey)
                .Replace("$MESSAGE", body)
                .Replace("$TITLE", title)
                .Replace("$URL", url)
                .Replace("'", "\"");
            
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.pushover.net/1/messages.json");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream())) {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                Debug.WriteLine($"<<<{result}>>>");
            }
        }
    }
}
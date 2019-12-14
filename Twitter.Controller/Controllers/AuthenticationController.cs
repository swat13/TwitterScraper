using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;

namespace Twitter.Controller.Controllers
{
    public class Authentication
    {
        public string RenewToken()
        {

            const string authHeaderFormat = "Basic {0}";

            var authHeader = string.Format(authHeaderFormat,
                Convert.ToBase64String(
                    Encoding.UTF8.GetBytes(Uri.EscapeDataString(ConfigurationManager.AppSettings.Get("Consumer_API_Keys")) + ":" +
                                           Uri.EscapeDataString(ConfigurationManager.AppSettings.Get("Consumer_API_Secret_Keys")))
                ));
            const string postBody = "grant_type=client_credentials";
            var authRequest = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings.Get("Twitter_Authentication_Url"));

            authRequest.Headers.Add("Authorization", authHeader);
            authRequest.Method = "POST";
            authRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            authRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            using (var stream = authRequest.GetRequestStream())
            {
                var content = Encoding.ASCII.GetBytes(postBody);
                stream.Write(content, 0, content.Length);
            }
            authRequest.Headers.Add("Accept-Encoding", "gzip");
            var authResponse = authRequest.GetResponse();
            using (authResponse)
            {
                using (var reader = new StreamReader(authResponse.GetResponseStream() ?? throw new InvalidOperationException()))
                {
                    var objectText = reader.ReadToEnd();
                    var json = JObject.Parse(objectText);
                    return json.GetValue("access_token").ToString();
                }
            }

        }

    }
}
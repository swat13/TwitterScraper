using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.ServiceModel;
using Twitter.Service.Services;

namespace Host
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            ExposeApi();

            new TwitterApiManagement().StartSearching();

            OpenBrowser();

            Console.ReadLine();
        }

        private static void OpenBrowser()
        {
            var path = Directory
                .GetParent(Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName).FullName)
                .FullName;
            System.Diagnostics.Process.Start(path + "/index.html");
        }

        private static void ExposeApi()
        {
            var services = new List<ServiceHost>
            {
                new ServiceHost(typeof(TwitterApiManagement))
            };

            foreach (var serviceHost in services)
            {
                serviceHost.Open();
                Console.WriteLine("Start Service :  " + serviceHost.Description.Name + " started");
            }
        }
    }
}
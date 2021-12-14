using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace WebServer
{
    public class Program
    {

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            //.UseUrls(@"https://localhost:1041", @"http://localhost:1040")
            .UseStartup<Startup>()
           ;

        private static bool isPortAvailable(int port)
        {

            var tcpConnInfoArray = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpConnections();

            foreach (TcpConnectionInformation tcpInfo in tcpConnInfoArray)
            {
                if (tcpInfo.LocalEndPoint.Port == port)
                {
                    Console.WriteLine($"Port: {port} is not available.");
                    return false;
                }
            }
            Console.WriteLine($"Port: {port} is available.");
            return true;
        }
    }
}

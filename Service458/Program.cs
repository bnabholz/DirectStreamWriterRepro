using Funq;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Service458
{
    class Program
    {
        static void Main(string[] args)
        {
            var appHost = new AppHost();
            appHost.Init();

            appHost.Start("http://+:80/Temporary_Listen_Addresses/myservice458/");

            Console.WriteLine("Press any key to make a test request or press ESCAPE to exit.");

            while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            {
                Console.WriteLine("Making request.");
                var sw = Stopwatch.StartNew();
                $"http://{Dns.GetHostName()}/Temporary_Listen_Addresses/myservice458/test".GetJsonFromUrl();
                sw.Stop();
                Console.WriteLine($"Request duration: {sw.ElapsedMilliseconds}ms");
            }

            appHost.Stop();
        }
    }

    class AppHost : AppSelfHostBase
    {
        public AppHost() : base("Test app with 4.5.8", typeof(AppHost).Assembly) { }

        public override void Configure(Container container) { }
    }

    class MyService : Service
    {
        [Route("/test", "GET")]
        public class MyRequest { }

        public class MyResponse
        {
            public List<string> Results { get; set; }
        }

        public MyResponse Get(MyRequest req)
        {
            return new MyResponse
            {
                Results = Enumerable.Range(1, 1000).Select(x => x.ToString()).ToList(),
            };
        }
    }
}

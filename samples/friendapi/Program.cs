using System;
using System.Web.Http.SelfHost;

namespace WebApiContrib.Formatting.CollectionJson
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var config = new HttpSelfHostConfiguration(new Uri("http://localhost:9200"));

            ServiceConfiguration.Configure(config);

            var host = new HttpSelfHostServer(config);

            host.OpenAsync().Wait();

            Console.WriteLine("Press any key to exit");
            Console.ReadLine();

            host.CloseAsync().Wait();
        }
    }
}
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Text;

namespace dpas.Web.Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            ILoggerFactory loggerFactory = new LoggerFactory().AddConsole().AddDebug();

            var host = new WebHostBuilder()
                .UseServer(new DPASServer(loggerFactory))//.UseKestrel()
                //.UseContentRoot(Directory.GetCurrentDirectory())
                //.UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}

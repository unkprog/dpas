using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace dpas.Web.Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseServer(new DPASServer()) //.UseKestrel()
                //.UseContentRoot(Directory.GetCurrentDirectory())
                //.UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}

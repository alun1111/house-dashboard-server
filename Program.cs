using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HouseDashboardServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.AddAWSProvider();

                    // When you need logging below set the minimum level.
                    // Otherwise the logging framework will default to
                    // Informational for external providers.
                    logging.SetMinimumLevel(LogLevel.Debug);
                })
                .Build()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

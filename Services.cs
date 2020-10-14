using HouseDashboardServer.Factories;
using HouseDashboardServer.Models;
using Microsoft.Extensions.DependencyInjection;

namespace HouseDashboardServer
{
    public static class Services
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ISummaryFactory<Summary>, SummaryFactory>();
        }
    }
}
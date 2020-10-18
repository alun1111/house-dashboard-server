using house_dashboard_server.Data;
using house_dashboard_server.Data.Interfaces;
using house_dashboard_server.Factories;
using house_dashboard_server.Models;
using Microsoft.Extensions.DependencyInjection;

namespace house_dashboard_server
{
    public static class Services
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ISummaryFactory<Summary>, SummaryFactory>();
            services.AddScoped<ISnapshotRangeFactory, SnapshotRangeRangeFactory>();
            services.AddScoped<IDynamoTableQueryRunner, DynamoTableQueryRunner>();
            
            services.AddScoped<IRainfallReadingsRepository, RainfallReadingsRepository>();
            services.AddScoped<IRiverLevelReadingsRepository, RiverLevelReadingsRepository>();
            services.AddScoped<IWeatherStationReadingRepository, WeatherStationReadingRepository>();
        }
    }
}
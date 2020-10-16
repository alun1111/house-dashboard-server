using HouseDashboardServer.Data;
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
            services.AddScoped<ISnapshotRangeFactory, SnapshotRangeRangeFactory>();
            services.AddScoped<IDynamoTableQueryRunner, DynamoTableQueryRunner>();
            
            services.AddScoped<IRainfallReadingsRepository, RainfallReadingsRepository>();
            services.AddScoped<IRiverLevelReadingsRepository, RiverLevelReadingsRepository>();
            services.AddScoped<IWeatherStationReadingRepository, WeatherStationReadingRepository>();
        }
    }
}
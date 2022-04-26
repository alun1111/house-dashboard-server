using house_dashboard_server.Data;
using house_dashboard_server.Data.DynamoDB;
using house_dashboard_server.Data.Factories;
using house_dashboard_server.Data.Models;
using house_dashboard_server.Formatters;
using Microsoft.Extensions.DependencyInjection;

namespace house_dashboard_server
{
    public static class Services
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ISummaryFactory<Summary>, SummaryFactory>();
            services.AddScoped<ISnapshotRangeFactory, SnapshotRangeFactory>();
            services.AddScoped<ISnapshotFlattener, SnapshotFlattener>();
            services.AddScoped<IDynamoTableQueryRunner, DynamoTableQueryRunner>();
            
            services.AddScoped<IRainfallReadingsRepository, RainfallReadingsRepository>();
            services.AddScoped<IRiverLevelReadingsRepository, RiverLevelReadingsRepository>();
            services.AddScoped<IWeatherStationReadingRepository, WeatherStationReadingRepository>();
        }
    }
}
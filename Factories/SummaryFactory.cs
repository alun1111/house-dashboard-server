using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using house_dashboard_server.Data.Interfaces;
using house_dashboard_server.Models;
using Microsoft.Extensions.Logging;

namespace house_dashboard_server.Factories
{
    public class SummaryFactory : ISummaryFactory<Summary>
    {
        private readonly ILogger<SummaryFactory> _logger;
        private readonly IRainfallReadingsRepository _rainfallReadingsRepository;
        private readonly IWeatherStationReadingRepository _weatherStationReadingRepository;

        private const string STATIONID = "wmr-89";
        private const string RAINFALLSTATIONID = "14881";
        private const string STATIONNAME = "WHITBURN";

        public SummaryFactory(ILogger<SummaryFactory> logger
            ,IRainfallReadingsRepository rainfallReadingsRepository
            ,IWeatherStationReadingRepository weatherStationReadingRepository)
        {
            _logger = logger;
            _rainfallReadingsRepository = rainfallReadingsRepository;
            _weatherStationReadingRepository = weatherStationReadingRepository;
        }

        public Summary Build()
        {
            _logger.Log(LogLevel.Information, "Building summary...");
            
            var rainfallSummaries = new List<RainfallSummary>();
            var temperatureSummary = new TemperatureSummary();
            
            Task.WaitAll(
                ApplyRainfallSummaries(rainfallSummaries), 
                ApplyTemperatureSummaries(temperatureSummary));

            return new Summary()
            {
                RainfallSummaries = rainfallSummaries,
                TemperatureSummary = temperatureSummary
            };
        }

        private async Task ApplyTemperatureSummaries(TemperatureSummary temperatureSummary)
        {
            var temperature 
                = await _weatherStationReadingRepository
                    .GetTemperatureReading(STATIONID, TemperatureReadingType.OUTSIDE);

            var today = temperature
                .Recent
                .Where(m => m.MeasurementTime >= DateTime.Today);

            var latest = today
                .OrderByDescending(m => m.MeasurementTime)
                .FirstOrDefault();

            if (today.Any())
            {
                temperatureSummary.Location = TemperatureReadingType.OUTSIDE.ToString();
                temperatureSummary.HighToday = today.Max(v => v.Value);
                temperatureSummary.LowToday = today.Min(v => v.Value);
                
                if (latest != null)
                {
                    temperatureSummary.Latest = latest.Value;
                    temperatureSummary.LatestMeasurementTime = latest.MeasurementTime;
                }
            }
        }

        private async Task ApplyRainfallSummaries(List<RainfallSummary> rainfallSummaries)
        {
            var stations = new[] 
                {("Whitburn", "14881"), 
                    ("Harperrig", "15200"),
                    ("Gogarbank", "15196")
                };
            
            foreach (var s in stations)
            {
                var rainfallSummary = new RainfallSummary();
                
                var rainfall 
                    = await _rainfallReadingsRepository.GetReading(s.Item2);

                rainfallSummary.LastThreeDays = rainfall
                    .Recent
                    .Sum(t => t.Value);

                var today = rainfall
                    .Recent
                    .Where(m => m.MeasurementTime >= DateTime.Today);

                if (today.Any())
                {
                    rainfallSummary.StationName = s.Item1; 
                    rainfallSummary.RainToday = today 
                        .Sum(t => t.Value);
                }
                
                rainfallSummaries.Add(rainfallSummary);
            }
        }
    }
}
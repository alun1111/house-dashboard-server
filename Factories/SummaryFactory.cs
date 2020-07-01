using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseDashboardServer.Data;
using HouseDashboardServer.Models;
using Microsoft.OpenApi.Extensions;

namespace HouseDashboardServer.Factories
{
    public class SummaryFactory : ISummaryFactory<Summary>
    {
        private RainfallReadingsRepository _rainfallReadingsRepository;
        private WeatherStationReadingRepository _weatherStationReadingsRepository;

        private const string STATIONID = "wmr-89";
        private const string RAINFALLSTATIONID = "14881";
        private const string STATIONNAME = "WHITBURN";

        public SummaryFactory()
        {
            _weatherStationReadingsRepository = new WeatherStationReadingRepository();
            _rainfallReadingsRepository = new RainfallReadingsRepository();
        }

        public Summary Build()
        {
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
                = await _weatherStationReadingsRepository
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
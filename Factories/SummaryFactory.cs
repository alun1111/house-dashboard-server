using System;
using System.Linq;
using System.Threading.Tasks;
using HouseDashboardServer.Data;
using HouseDashboardServer.Models;

namespace HouseDashboardServer.Factories
{
    public class SummaryFactory : ISummaryFactory<Summary>
    {
        private RainfallReadingsRepository _rainfallReadingsRepository;
        private WeatherStationReadingRepository _weatherStationReadingsRepository;
        
        private const string STATIONID = "WMR-89";
        private const string RAINFALLSTATIONID = "14881";

        public SummaryFactory()
        {
            _weatherStationReadingsRepository = new WeatherStationReadingRepository();
            _rainfallReadingsRepository = new RainfallReadingsRepository();
        }

        public Summary Build(string id)
        {
            var summary = new Summary();
            
            Task.WaitAll(new []{
                new Task(() => ApplyRainfallSummaries(summary)),
                new Task(() => ApplyTemperatureSummaries(summary))
                    });

            return summary;
        }

        private async void ApplyTemperatureSummaries(Summary summary)
        {
            var temperature 
                = await _weatherStationReadingsRepository
                    .GetTemperatureReading(STATIONID, TemperatureReadingType.OUTSIDE);
            
           var temperatureSummary = new TemperatureSummary();
           
           summary.TemperatureSummary = temperatureSummary;
        }

        private async void ApplyRainfallSummaries(Summary summary)
        {
            var rainfall 
                = await _rainfallReadingsRepository.GetReading(RAINFALLSTATIONID);

            var rainfallSummary = new RainfallSummary
            {
                LastThreeDays = rainfall
                    .Recent
                    .Sum(t => t.Value),
                RainToday = rainfall
                    .Recent
                    .Where(m => m.MeasurementTime >= DateTime.Today)
                    .Sum(t => t.Value)
            };



            summary.RainfallSummary = rainfallSummary;
        }
    }
}
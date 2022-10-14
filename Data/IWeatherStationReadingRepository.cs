using System;
using System.Threading.Tasks;
using house_dashboard_server.Data.Models;

namespace house_dashboard_server.Data
{
    public interface IWeatherStationReadingRepository
    {
        Task<Reading<decimal>> GetTemperatureReading(string stationId
            , TemperatureReadingType type
            , DateTime dateFrom = default);
    }
}
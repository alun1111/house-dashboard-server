using System.Threading.Tasks;
using HouseDashboardServer.Models;

namespace HouseDashboardServer.Data
{
    public interface IWeatherStationReadingRepository
    {
        Task<Reading<decimal>> GetTemperatureReading(string stationId, TemperatureReadingType type);
    }
}
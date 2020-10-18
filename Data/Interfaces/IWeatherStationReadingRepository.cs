using System.Threading.Tasks;
using house_dashboard_server.Models;

namespace house_dashboard_server.Data.Interfaces
{
    public interface IWeatherStationReadingRepository
    {
        Task<Reading<decimal>> GetTemperatureReading(string stationId, TemperatureReadingType type);
    }
}
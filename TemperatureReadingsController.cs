using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HouseDashboardServer.Data;
using HouseDashboardServer.Models;

namespace HouseDashboardServer
{
    [ApiController]
    [Route("[controller]")]
    public class TemperatureReadingsController : ControllerBase
    {
        private readonly WeatherStationReadingRepository _weatherReadingsRepository;

        public TemperatureReadingsController()
        {
            _weatherReadingsRepository = new WeatherStationReadingRepository();
        }

        [EnableCors("default-policy")]
        [HttpGet("{id}/inside")]
        public Task<NumberReading<decimal>> GetInside(string id) 
            => _weatherReadingsRepository.GetTemperatureReading(id
                , TemperatureReadingType.INSIDE);

        [EnableCors("default-policy")]
        [HttpGet("{id}/outside")]
        public Task<NumberReading<decimal>> GetOutside(string id)
            => _weatherReadingsRepository.GetTemperatureReading(id
                , TemperatureReadingType.OUTSIDE);
    }
}

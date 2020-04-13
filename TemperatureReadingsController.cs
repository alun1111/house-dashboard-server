using house_dashboard_server.Data;
using house_dashboard_server.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace house_dashboard_server
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
        [HttpGet]
        public async Task<ReadingSet<decimal>> Get() 
            => await _weatherReadingsRepository.GetReadingSet();

        [EnableCors("default-policy")]
        [HttpGet("{id}/inside")]
        public async Task<NumberReading<decimal>> GetInside(string id) 
            => await _weatherReadingsRepository.GetTemperatureReading(id, TemperatureReadingType.INSIDE);
        
        [EnableCors("default-policy")]
        [HttpGet("{id}/outside")]
        public async Task<NumberReading<decimal>> GetOutside(string id) 
            => await _weatherReadingsRepository.GetTemperatureReading(id, TemperatureReadingType.OUTSIDE);
    }
}

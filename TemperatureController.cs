using System.Threading.Tasks;
using house_dashboard_server.Data.Interfaces;
using house_dashboard_server.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace house_dashboard_server
{
    [ApiController]
    [Route("[controller]")]
    public class TemperatureController : ControllerBase
    {
        private readonly IWeatherStationReadingRepository _weatherStationReadingRepository;

        public TemperatureController(IWeatherStationReadingRepository weatherStationReadingRepository)
        {
            _weatherStationReadingRepository = weatherStationReadingRepository;
        }

        [EnableCors("default-policy")]
        [HttpGet("{id}/inside")]
        public Task<Reading<decimal>> GetInside(string id) 
            => _weatherStationReadingRepository.GetTemperatureReading(id
                , TemperatureReadingType.INSIDE);

        [EnableCors("default-policy")]
        [HttpGet("{id}/outside")]
        public Task<Reading<decimal>> GetOutside(string id)
            => _weatherStationReadingRepository.GetTemperatureReading(id
                , TemperatureReadingType.OUTSIDE);
    }
}

using System.Threading.Tasks;
using house_dashboard_server.Data;
using house_dashboard_server.Data.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace house_dashboard_server
{
    [ApiController]
    [Route("[controller]")]
    public class TemperatureController : ControllerBase
    {
        private readonly IWeatherStationReadingRepository _weatherStationReadingRepository;
        private readonly string _apiKey;

        public TemperatureController(IConfiguration configuration, 
            IWeatherStationReadingRepository weatherStationReadingRepository)
        {
            _apiKey = configuration["ApiKey"];
            _weatherStationReadingRepository = weatherStationReadingRepository;
        }

        [EnableCors("default-policy")]
        [HttpGet("{id}/inside")]
        public async Task<IActionResult> GetInside([FromHeader]string authorisation, string id)
        {
            if (authorisation != _apiKey)
                return Unauthorized();
            
            return Ok(await _weatherStationReadingRepository.GetTemperatureReading(id
                , TemperatureReadingType.INSIDE));
        }

        [EnableCors("default-policy")]
        [HttpGet("{id}/outside")]
        public async Task<IActionResult> GetOutside([FromHeader]string authorisation, string id)
        {
            if (authorisation != _apiKey)
                return Unauthorized();
            
            return Ok(await _weatherStationReadingRepository.GetTemperatureReading(id
                , TemperatureReadingType.OUTSIDE));
        }
    }
}

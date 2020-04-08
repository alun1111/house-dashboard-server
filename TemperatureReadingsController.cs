using house_dashboard_server.Data;
using house_dashboard_server.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
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
        [HttpGet("{id}")]
        public async Task<NumberReading<decimal>> Get(string id) 
            => await _weatherReadingsRepository.GetReading(id);
    }
}

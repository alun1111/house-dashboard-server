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
    public class WeatherStationReadingsController : ControllerBase
    {
        private readonly WeatherStationReadingRepository _readingSetRepository;

        public WeatherStationReadingsController()
        {
            _readingSetRepository = new WeatherStationReadingRepository();
        }

        [EnableCors("default-policy")]
        [HttpGet]
        public async Task<ReadingSet> Get() => await _readingSetRepository.GetReadingSet();
    }
}

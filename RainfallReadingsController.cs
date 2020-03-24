using System.Threading.Tasks;
using house_dashboard_server.Data;
using house_dashboard_server.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace house_dashboard_server
{
    [ApiController]
    [Route("[controller]")]
    public class RainfallController : ControllerBase
    {
        private readonly RainfallReadingsRepository _rainfallRepository;

        public RainfallController()
        {
            _rainfallRepository = new RainfallReadingsRepository();
        }

        [EnableCors("default-policy")]
        [HttpGet]
        public async Task<ReadingSet<decimal>> Get() 
            => await _rainfallRepository.GetReadingSet();

        [EnableCors("default-policy")]
        [HttpGet("{id}")]
        public async Task<NumberReading<decimal>> Get(string id) 
            => await _rainfallRepository.GetReading(id);
    }
}
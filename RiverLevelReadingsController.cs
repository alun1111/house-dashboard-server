using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HouseDashboardServer.Data;
using HouseDashboardServer.Models;

namespace HouseDashboardServer
{
    [ApiController]
    [Route("[controller]")]
    public class RiverLevelReadingsController : ControllerBase
    {
        private readonly RiverLevelReadingsRepository _readingSetRepository;

        public RiverLevelReadingsController()
        {
            _readingSetRepository = new RiverLevelReadingsRepository();
        }

        [EnableCors("default-policy")]
        [HttpGet("{id}")]
        public async Task<NumberReading<decimal>> Get(string id) 
            => await _readingSetRepository.GetReading(id);
    }
}

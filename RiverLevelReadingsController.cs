using house_dashboard_server.Data;
using house_dashboard_server.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace house_dashboard_server
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
        public async Task<ReadingSet<decimal>> Get(string id) 
            => await _readingSetRepository.GetReadingSet(id);
    }
}

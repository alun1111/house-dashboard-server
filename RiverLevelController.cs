using System;
using System.Threading.Tasks;
using house_dashboard_server.Data;
using house_dashboard_server.Data.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace house_dashboard_server
{
    [ApiController]
    [Route("[controller]")]
    public class RiverLevelController : ControllerBase
    {
        private readonly IRiverLevelReadingsRepository _riverLevelReadingsRepository;

        public RiverLevelController(IRiverLevelReadingsRepository riverLevelReadingsRepository)
        {
            _riverLevelReadingsRepository = riverLevelReadingsRepository;
        }

        [EnableCors("default-policy")]
        [HttpGet("{id}")]
        public Task<Reading<decimal>> Get(string id, DateTime dateFrom) 
            => _riverLevelReadingsRepository.GetReading(id, dateFrom);
    }
}

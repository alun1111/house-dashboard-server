using System;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HouseDashboardServer.Data;
using HouseDashboardServer.Models;

namespace HouseDashboardServer
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

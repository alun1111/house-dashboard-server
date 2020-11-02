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
    public class RainfallController : ControllerBase
    {
        private readonly IRainfallReadingsRepository _rainfallReadingsRepository;

        public RainfallController(IRainfallReadingsRepository rainfallReadingsRepository)
        {
            _rainfallReadingsRepository = rainfallReadingsRepository;
        }

        [EnableCors("default-policy")]
        [HttpGet("{id}")]
        public Task<Reading<decimal>> Get(string id, DateTime dateFrom)
        {
           return _rainfallReadingsRepository.GetReading(id, dateFrom); 
        }
    }
}
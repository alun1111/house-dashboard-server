using System;
using System.Threading.Tasks;
using HouseDashboardServer.Data;
using HouseDashboardServer.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HouseDashboardServer
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
        public Task<NumberReading<decimal>> Get(string id, DateTime dateFrom)
        {
           return _rainfallReadingsRepository.GetReading(id, dateFrom); 
        }
    }
}
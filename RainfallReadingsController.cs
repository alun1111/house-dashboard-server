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
        private readonly RainfallReadingsRepository _rainfallRepository;

        public RainfallController()
        {
            _rainfallRepository = new RainfallReadingsRepository();
        }

        [EnableCors("default-policy")]
        [HttpGet("{id}")]
        public Task<NumberReading<decimal>> Get(string id, DateTime dateFrom)
        {
           return _rainfallRepository.GetReading(id, dateFrom); 
        }
    }
}
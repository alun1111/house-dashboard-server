using System;
using System.Threading.Tasks;
using house_dashboard_server.Data;
using house_dashboard_server.Data.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace house_dashboard_server
{
    [ApiController]
    [Route("[controller]")]
    public class RiverLevelController : ControllerBase
    {
        private readonly IRiverLevelReadingsRepository _riverLevelReadingsRepository;
        private readonly string _apiKey;

        public RiverLevelController(IConfiguration configuration,
            IRiverLevelReadingsRepository riverLevelReadingsRepository)
        {
            _apiKey = configuration["ApiKey"];
            _riverLevelReadingsRepository = riverLevelReadingsRepository;
        }

        [EnableCors("default-policy")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromHeader]string authorisation, 
            string id, DateTime dateFrom)
        {
            if (authorisation != _apiKey)
                return Unauthorized();
            
            return Ok(await _riverLevelReadingsRepository.GetReading(id, dateFrom));
        }
    }
}

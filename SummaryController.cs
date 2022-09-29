using System.Collections;
using house_dashboard_server.Data.Factories;
using house_dashboard_server.Data.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace house_dashboard_server
{
    [ApiController]
    [Route("[controller]")]
    public class SummaryController : ControllerBase
    {
        private readonly ISummaryFactory<Summary> _summaryFactory;
        private readonly string _apiKey;

        public SummaryController(IConfiguration configuration, 
            ISummaryFactory<Summary> summaryFactory)
        {
            _apiKey = configuration["ApiKey"];
            _summaryFactory = summaryFactory;
        }

        [EnableCors("default-policy")]
        [HttpGet]
        public IActionResult Get([FromHeader]string authorisation)
        {
            if (authorisation != _apiKey)
                return Unauthorized();
            
            return Ok(_summaryFactory.Build());
        }
    }
}
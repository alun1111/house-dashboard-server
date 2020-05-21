using System.Threading.Tasks;
using HouseDashboardServer.Factories;
using HouseDashboardServer.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HouseDashboardServer
{
    [ApiController]
    [Route("[controller]")]
    public class SummaryController : ControllerBase
    {
        private readonly ISummaryFactory<Summary> _summaryFactory;

        public SummaryController()
        {
            _summaryFactory = new SummaryFactory();
        }

        [EnableCors("default-policy")]
        [HttpGet("{id}")]
        [HttpGet]
        public async Task<Summary> Get(string id) 
            => await _summaryFactory.Build(id);
    }
}
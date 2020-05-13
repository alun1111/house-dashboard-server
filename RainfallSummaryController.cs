using System.Threading.Tasks;
using HouseDashboardServer.Factories;
using HouseDashboardServer.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HouseDashboardServer
{
    [ApiController]
    [Microsoft.AspNetCore.Components.Route("[controller]")]
    public class RainfallSummaryController : ControllerBase
    {
        private readonly ISummaryFactory<RainfallSummary> _rainfallSummaryFactory;

        public RainfallSummaryController()
        {
            _rainfallSummaryFactory = new RainfallSummaryFactory();
        }

        [EnableCors("default-policy")]
        [HttpGet]
        public Task<RainfallSummary> Get() 
            => _rainfallSummaryFactory.Build();
    }
}
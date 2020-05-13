using System.Threading.Tasks;
using house_dashboard_server.Data;
using house_dashboard_server.Factories;
using house_dashboard_server.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace house_dashboard_server
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
        public async Task<RainfallSummary> Get() 
            => await _rainfallSummaryFactory.Build();
    }
}
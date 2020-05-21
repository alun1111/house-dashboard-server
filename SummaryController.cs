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
        [HttpGet]
        public Summary Get() 
            => _summaryFactory.Build();
    }
}
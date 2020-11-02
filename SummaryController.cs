using house_dashboard_server.Data.Factories;
using house_dashboard_server.Data.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace house_dashboard_server
{
    [ApiController]
    [Route("[controller]")]
    public class SummaryController : ControllerBase
    {
        private readonly ISummaryFactory<Summary> _summaryFactory;

        public SummaryController(ISummaryFactory<Summary> summaryFactory)
        {
            _summaryFactory = summaryFactory;
        }

        [EnableCors("default-policy")]
        [HttpGet]
        public Summary Get() 
            => _summaryFactory.Build();
    }
}
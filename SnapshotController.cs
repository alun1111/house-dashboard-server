using System.Collections.Generic;
using house_dashboard_server.Data.Factories;
using house_dashboard_server.Formatters;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace house_dashboard_server
{
    [ApiController]
    [Route("[controller]")]
    public class SnapshotController : Controller
    {
        private readonly ISnapshotRangeFactory _snapshotRangeFactory;
        private readonly ISnapshotFlattener _snapshotFlattener;
        private readonly string _apiKey;

        public SnapshotController(IConfiguration configuration,
            ISnapshotRangeFactory snapshotRangeFactory, 
            ISnapshotFlattener snapshotFlattener)
        {
            _apiKey = configuration["ApiKey"];
            _snapshotRangeFactory = snapshotRangeFactory;
            _snapshotFlattener = snapshotFlattener;
        }
        
        [EnableCors("default-policy")]
        [HttpGet]
        public IActionResult Get([FromHeader]string authorisation)
        {
            if (authorisation != _apiKey)
                return Unauthorized();
            
           var snapshots = _snapshotRangeFactory.Build();
           return Ok(_snapshotFlattener.Flatten(snapshots));
        }
    }
}
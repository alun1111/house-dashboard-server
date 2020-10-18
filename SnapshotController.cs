using System.Collections.Generic;
using house_dashboard_server.Factories;
using house_dashboard_server.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace house_dashboard_server
{
    [ApiController]
    [Route("[controller]")]
    public class SnapshotController : Controller
    {
        private readonly ISnapshotRangeFactory _snapshotRangeFactory;

        public SnapshotController(ISnapshotRangeFactory snapshotRangeFactory)
        {
            _snapshotRangeFactory = snapshotRangeFactory;
        }
        
        // GET
        [EnableCors("default-policy")]
        [HttpGet]
        public Dictionary<string, HashSet<SnapshotItem>> Get()
            => _snapshotRangeFactory.Build();
    }
}
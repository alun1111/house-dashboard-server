using System.Collections.Generic;
using house_dashboard_server.Data.Factories;
using house_dashboard_server.Data.Models;
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

        [EnableCors("default-policy")]
        [HttpGet]
        public Dictionary<string, List<SnapshotItem>> Get()
        {
            // need to flatten out the snapshots and return just a list to work with
            // text/csv
           var snapshots =  _snapshotRangeFactory.Build();
           
           return snapshots;

        }
    }
}
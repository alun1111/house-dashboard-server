using System.Collections.Generic;
using house_dashboard_server.Data.Factories;
using house_dashboard_server.Formatters;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace house_dashboard_server
{
    [ApiController]
    [Route("[controller]")]
    public class SnapshotController : Controller
    {
        private readonly ISnapshotRangeFactory _snapshotRangeFactory;
        private readonly ISnapshotFlattener _snapshotFlattener;

        public SnapshotController(ISnapshotRangeFactory snapshotRangeFactory, 
            ISnapshotFlattener snapshotFlattener)
        {
            _snapshotRangeFactory = snapshotRangeFactory;
            _snapshotFlattener = snapshotFlattener;
        }
        
        [EnableCors("default-policy")]
        [HttpGet]
        public List<string[]> Get()
        {
           var snapshots = _snapshotRangeFactory.Build();
           return _snapshotFlattener.Flatten(snapshots);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using HouseDashboardServer.Factories;
using HouseDashboardServer.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HouseDashboardServer
{
    [ApiController]
    [Route("[controller]")]
    public class SnapshotController : Controller
    {
        private readonly ISnapshotRangeFactory _snapshotRangeFactory;

        public SnapshotController()
        {
            _snapshotRangeFactory = new SnapshotRangeRangeFactory();
        }
        
        // GET
        [EnableCors("default-policy")]
        [HttpGet]
        public IEnumerable<Snapshot> Get()
            => _snapshotRangeFactory.Build();
    }
}
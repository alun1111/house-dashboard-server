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
﻿using System;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HouseDashboardServer.Data;
using HouseDashboardServer.Models;

namespace HouseDashboardServer
{
    [ApiController]
    [Route("[controller]")]
    public class RiverLevelController : ControllerBase
    {
        private readonly RiverLevelReadingsRepository _readingSetRepository;

        public RiverLevelController()
        {
            _readingSetRepository = new RiverLevelReadingsRepository();
        }

        [EnableCors("default-policy")]
        [HttpGet("{id}")]
        public Task<Reading<decimal>> Get(string id, DateTime dateFrom) 
            => _readingSetRepository.GetReading(id, dateFrom);
    }
}
using house_dashboard_server.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace house_dashboard_server
{
    [ApiController]
    [Route("measurement")]
    public class MeasurementController : ControllerBase
    {
        [HttpGet]
        public IList<Measurement> Get()
        {
            return new List<Measurement>()
            {
                new Measurement
                {
                    Name = "OutsideTemperature",
                    Current = 40.2M,
                    Recent = new List<decimal>() { 40M, 40M, 40M, 41M, 41M, 42M }
                }
            };
        }
    }
}

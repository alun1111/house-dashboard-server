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
        public string Get()
        {
            return "HELLO";
        }
    }
}

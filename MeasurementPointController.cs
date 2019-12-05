using house_dashboard_server.Data;
using house_dashboard_server.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace house_dashboard_server
{
    [ApiController]
    [Route("[controller]")]
    public class MeasurementPointController : ControllerBase
    {
        private MeasurementPointRepository _measurementPointRepository;

        public MeasurementPointController()
        {
            _measurementPointRepository = new MeasurementPointRepository();
        }

        [HttpGet]
        public async Task<MeasurementPoint> Get()
        {
            return await _measurementPointRepository
                .GetMeasurementPoint()
                .ConfigureAwait(false);
        }
    }
}

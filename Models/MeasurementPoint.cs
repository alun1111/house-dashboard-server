using System;
using System.Collections.Generic;

namespace house_dashboard_server.Models
{
    public class MeasurementPoint
    {
        public DateTime MeasurementTime { get; set; }
        public IList<Measurement> Measurements { get; set; }
    }
}

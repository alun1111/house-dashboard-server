using System;
using System.Collections.Generic;

namespace house_dashboard_server.Models
{
    public class MeasurementPoint
    {
        public DateTime ReportingTime { get; set; }
        public IList<Measurement<decimal>> Measurements { get; set; }
    }
}

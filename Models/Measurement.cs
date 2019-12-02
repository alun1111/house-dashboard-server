using System;
using System.Collections.Generic;
using System.Text;

namespace house_dashboard_server.Models
{
    public class Measurement
    {
        public string Name { get; set; }
        public decimal Current { get; set; }
        public IList<decimal> Recent { get; set; }
    }

    public class MeasurementPoint
    {
        public DateTime MeasurementTime { get; set; }
        public IList<Measurement> Measurements { get; set; }
    }
}

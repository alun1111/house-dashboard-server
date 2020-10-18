using System;
using System.Collections.Generic;

namespace house_dashboard_server.Models
{
    public class Summary
    {
        public List<RainfallSummary> RainfallSummaries { get; set; }
        public TemperatureSummary TemperatureSummary { get; set; }
    }
    
    public class RainfallSummary
    {
        public string StationName { get; set; }
        public decimal RainToday { get; set; }
        public decimal LastThreeDays { get; set; }
    }

    public class TemperatureSummary
    {
        public string Location { get; set; }
        public decimal HighToday { get; set; }
        public decimal LowToday { get; set; }
        
        public decimal Latest { get; set; }
        
        public DateTime LatestMeasurementTime { get; set; }
    }
}
using System;

namespace HouseDashboardServer.Models
{
    public class Summary
    {
        public RainfallSummary RainfallSummary { get; set; }
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
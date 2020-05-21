namespace HouseDashboardServer.Models
{
    public class Summary
    {
        public RainfallSummary RainfallSummary { get; set; }
        public TemperatureSummary TemperatureSummary { get; set; }
    }
    
    public class RainfallSummary
    {
        public decimal RainToday { get; set; }
        public decimal LastThreeDays { get; set; }
    }

    public class TemperatureSummary
    {
        public decimal HighToday { get; set; }
        
        public decimal LowToday { get; set; }
    }
}
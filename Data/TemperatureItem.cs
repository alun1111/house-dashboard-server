using System;

namespace house_dashboard_server.Data
{
    public class TemperatureItem : IDynamoDbItem
    {
        public TemperatureItem(DateTime measurementTime, decimal outsideTemperature)
        {
            MeasurementTime = measurementTime;
            Value = outsideTemperature;
        }

        public DateTime MeasurementTime { get; }
        public decimal Value { get; }
    }
}

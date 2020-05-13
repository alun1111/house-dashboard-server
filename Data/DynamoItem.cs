using System;

namespace HouseDashboardServer.Data
{
    public class DynamoDbItem<T> : IDynamoDbItem<T>
    {
        public DynamoDbItem(DateTime measurementTime, long timeIndex, T value)
        {
            MeasurementTime = measurementTime;
            TimeIndex = timeIndex;
            Value = value;
        }

        public DateTime MeasurementTime { get; }
        
        public long TimeIndex { get; }
        
        public T Value { get; }
    }
}

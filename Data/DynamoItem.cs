using System;

namespace house_dashboard_server.Data
{
    public class DynamoDbItem<T> : IDynamoDbItem<T>
    {
        public DynamoDbItem(DateTime measurementTime, T value)
        {
            MeasurementTime = measurementTime;
            Value = value;
        }

        public DateTime MeasurementTime { get; }
        public T Value { get; }
    }
}

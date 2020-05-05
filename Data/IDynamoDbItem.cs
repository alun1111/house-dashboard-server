using System;

namespace house_dashboard_server.Data
{
    public interface IDynamoDbItem<T>
    {
        DateTime MeasurementTime { get; }
        long TimeIndex { get; }
        T Value { get; }
    }
}
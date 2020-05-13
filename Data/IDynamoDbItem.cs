using System;

namespace HouseDashboardServer.Data
{
    public interface IDynamoDbItem<T>
    {
        DateTime MeasurementTime { get; }
        long TimeIndex { get; }
        T Value { get; }
    }
}
using System;

namespace house_dashboard_server.Data.DynamoDB
{
    public interface IMeasurement<T>
    {
        DateTime MeasurementTime { get; }
        long TimeIndex { get; }
        T Value { get; }
    }
}
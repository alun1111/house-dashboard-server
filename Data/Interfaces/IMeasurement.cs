using System;

namespace house_dashboard_server.Data.Interfaces
{
    public interface IMeasurement<T>
    {
        DateTime MeasurementTime { get; }
        long TimeIndex { get; }
        T Value { get; }
    }
}
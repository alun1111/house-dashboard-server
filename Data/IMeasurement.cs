using System;

namespace HouseDashboardServer.Data
{
    public interface IMeasurement<T>
    {
        DateTime MeasurementTime { get; }
        long TimeIndex { get; }
        T Value { get; }
    }
}
using System;

namespace house_dashboard_server.Data
{
    public interface IDynamoDbItem
    {
        DateTime MeasurementTime { get; }
        decimal Value { get; }
    }
}
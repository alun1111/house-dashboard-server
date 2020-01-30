using house_dashboard_server.Models;
using System.Collections.Generic;
using System.Linq;

namespace house_dashboard_server.Data
{
    public class NumberReadingFactory
    {
        public Reading<decimal> BuildReading(string measurementName, 
            List<DynamoDbItem<decimal>> reducedScanResult)
        {
            var orderedScanResult = reducedScanResult.OrderBy(x => x.MeasurementTime);
            var latestMeasurement = orderedScanResult.LastOrDefault();

            return new Reading<decimal>(
                name: measurementName,
                current: new DynamoDbItem<decimal>(
                    latestMeasurement.MeasurementTime,
                    latestMeasurement.Value),
                recent: orderedScanResult
                    .Skip(1)
                    .Select(s => new DynamoDbItem<decimal>(s.MeasurementTime, s.Value) as IDynamoDbItem<decimal>)
                    .ToList()
                    );
        }

    }
}

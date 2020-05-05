using house_dashboard_server.Models;
using System.Collections.Generic;
using System.Linq;

namespace house_dashboard_server.Data
{
    public class NumberReadingFactory
    {
        public NumberReading<decimal> BuildReading(string measurementName, 
            List<DynamoDbItem<decimal>> reducedScanResult)
        {
            var orderedScanResult 
                = reducedScanResult.OrderBy(x => x.MeasurementTime);
            
            var latestMeasurement = orderedScanResult.LastOrDefault();

            return new NumberReading<decimal>(
                name: measurementName,
                current: new DynamoDbItem<decimal>(
                    latestMeasurement.MeasurementTime,
                    latestMeasurement.TimeIndex,
                    latestMeasurement.Value),
                recent: orderedScanResult
                    .Skip(1)
                    .Select(s => new DynamoDbItem<decimal>(s.MeasurementTime, s.TimeIndex, s.Value) as IDynamoDbItem<decimal>)
                    .ToList()
                    );
        }

    }
}

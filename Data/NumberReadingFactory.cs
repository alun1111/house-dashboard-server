using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseDashboardServer.Models;

namespace HouseDashboardServer.Data
{
    public class NumberReadingFactory
    {
        public Task<NumberReading<decimal>> BuildReading(string measurementName, 
            List<DynamoDbItem<decimal>> reducedScanResult)
        {
            var orderedScanResult 
                = reducedScanResult.OrderBy(x => x.MeasurementTime);
            
            var latestMeasurement = orderedScanResult.LastOrDefault();
            var reading =  new NumberReading<decimal>(
                                name: measurementName,
                                current: latestMeasurement,
                                recent: orderedScanResult
                                    .Select(s 
                                        => new DynamoDbItem<decimal>(
                                            s.MeasurementTime
                                            , s.TimeIndex
                                            , s.Value) as IDynamoDbItem<decimal>)
                                    .ToList()
                                    );

            return new Task<NumberReading<decimal>>(() =>
            {
                return reading;
            });
        }

    }
}

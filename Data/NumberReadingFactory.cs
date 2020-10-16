using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseDashboardServer.Models;

namespace HouseDashboardServer.Data
{
    public class NumberReadingFactory
    {
        public Reading<decimal> BuildReading(string measurementName, 
            List<IMeasurement<decimal>> reducedScanResult)
        {
            var orderedScanResult 
                = reducedScanResult.OrderBy(x => x.MeasurementTime);
            
            var latestMeasurement = orderedScanResult.LastOrDefault();
            return new Reading<decimal>(
                                name: measurementName,
                                current: latestMeasurement,
                                recent: orderedScanResult
                                    .Select(s 
                                        => new Measurement<decimal>(
                                            s.MeasurementTime
                                            , s.TimeIndex
                                            , s.Value) as IMeasurement<decimal>)
                                    .ToList()
                                    );
        }

    }
}

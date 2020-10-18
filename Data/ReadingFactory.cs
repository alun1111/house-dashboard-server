using System.Collections.Generic;
using System.Linq;
using house_dashboard_server.Data.Interfaces;
using house_dashboard_server.Models;

namespace house_dashboard_server.Data
{
    public class ReadingFactory
    {
        public static Reading<decimal> BuildReading(string measurementName, 
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

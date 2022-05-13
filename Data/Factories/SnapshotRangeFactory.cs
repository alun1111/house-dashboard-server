using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using house_dashboard_server.Data.DynamoDB;
using house_dashboard_server.Data.Models;
using Microsoft.Extensions.Logging;

namespace house_dashboard_server.Data.Factories
{
    public class SnapshotRangeFactory : ISnapshotRangeFactory
    {
        private readonly IRainfallReadingsRepository _rainfallReadingsRepository;
        private readonly IRiverLevelReadingsRepository _riverLevelReadingsRepository;
        private readonly ILogger<SnapshotRangeFactory> _logger;
        private readonly IFormatProvider _culture 
            = CultureInfo.CreateSpecificCulture("en-GB");
        
        public SnapshotRangeFactory(IRainfallReadingsRepository rainfallReadingsRepository
            , IRiverLevelReadingsRepository riverLevelReadingsRepository
            , ILogger<SnapshotRangeFactory> logger)
        {
            _rainfallReadingsRepository = rainfallReadingsRepository;
            _riverLevelReadingsRepository = riverLevelReadingsRepository;
            _logger = logger;
        }

        /// <summary>
        /// Build a dictionary of readings (snapshotItems) keyed by DateTime
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, List<SnapshotItem>> Build()
        {
            var output = new Dictionary<string, List<SnapshotItem>>();
            
            var readings = RetrieveReadingsSet(); 
            
            try
            {
                // Wait for all the tasks to finish.
                _logger.Log(LogLevel.Debug, "Starting WaitAll: " 
                                            + DateTime.Now.ToString("hh:mm:ss.fff", _culture));
                Task.WaitAll(readings.Select(t=>t.Value).ToArray());
            }
            catch (AggregateException e)
            {
                throw e.Flatten();
            }
            
            foreach (var (key, value) in readings)
            {
                if (value is Task<List<IMeasurement<decimal>>> val)
                {
                    
                    _logger.Log(LogLevel.Debug, "Getting Result: " 
                                            + DateTime.Now.ToString("hh:mm:ss.fff", _culture));
                    TryAddSnapshotItem(val.Result, output, key);
                }
            }

            return output;
        }

        private Dictionary<string, Task> RetrieveReadingsSet()
        {
            // This method should be replaced with some way of feeding in the
            // intended id's
            var tasks = new Dictionary<string, Task>();
            
            var riverStationIds = new []
            {
                ("Whitburn", "14881"), 
                ("Almondell", "14869"), 
                ("Cragiehall", "14867")
            };
            
            var rainfallStationIds = new []
            {
                ("Harperrig", "15200"),
                ("Whitburn", "14881"), 
                ("Gogarbank","15196")
            };

            foreach (var id in riverStationIds)
            {
                tasks.Add("River Level - " + id.Item1, _riverLevelReadingsRepository.GetReadingItems(id.Item2));
            }

            foreach (var id in rainfallStationIds)
            {
                tasks.Add("Rainfall - " + id.Item1, _rainfallReadingsRepository.GetMeasurements(id.Item2));
            }

            return tasks;
        }

        private void TryAddSnapshotItem(List<IMeasurement<decimal>> dynamoResult, 
            Dictionary<string, List<SnapshotItem>> output,
            string label)
        {
            foreach (var r in dynamoResult)
            {
                var roundedTime = RoundedTime(r);

                if (output.TryGetValue(roundedTime, out var itemsOnDate))
                {
                    itemsOnDate.Add(new SnapshotItem { Description = label, Value = r.Value });
                }
                else
                {
                    itemsOnDate = new List<SnapshotItem>
                    {
                        new() {Description = label, Value = r.Value}
                    };
                    output.Add(roundedTime, itemsOnDate);
                }
            }
        }

        private static string RoundedTime(IMeasurement<decimal> r)
        {
            var roundedTime = new DateTime(r.MeasurementTime.Year,
                r.MeasurementTime.Month,
                r.MeasurementTime.Day,
                r.MeasurementTime.Hour,
                r.MeasurementTime.Minute,
                0).ToString();
            return roundedTime;
        }
    }
}
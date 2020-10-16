using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using HouseDashboardServer.Data;
using HouseDashboardServer.Models;

namespace HouseDashboardServer.Factories
{
    public class SnapshotRangeRangeFactory : ISnapshotRangeFactory
    {
        private readonly IRainfallReadingsRepository _rainfallReadingsRepository;
        private readonly IRiverLevelReadingsRepository _riverLevelReadingsRepository;

        private readonly IFormatProvider _culture 
            = CultureInfo.CreateSpecificCulture("en-GB");

        public SnapshotRangeRangeFactory(IRainfallReadingsRepository rainfallReadingsRepository
            ,IRiverLevelReadingsRepository riverLevelReadingsRepository)
        {
            _rainfallReadingsRepository = rainfallReadingsRepository;
            _riverLevelReadingsRepository = riverLevelReadingsRepository;
        }

        public Dictionary<string, HashSet<SnapshotItem>> Build()
        {
            var output = new Dictionary<string, HashSet<SnapshotItem>>();
            
            Task<List<IMeasurement<decimal>>> rainfallLevels 
                = _rainfallReadingsRepository.GetMeasurements("14881");
            
            Task<List<IMeasurement<decimal>>> riverLevels 
                = _riverLevelReadingsRepository.GetReadingItems("14881-SG");

            Task.WaitAll(rainfallLevels, riverLevels);

            TryAddSnapshotItem(rainfallLevels.Result, output, "Whitburn - Rainfall");
            TryAddSnapshotItem(riverLevels.Result, output, "Whitburn - RiverLevel");
            
            return output;
        }

        private void TryAddSnapshotItem(List<IMeasurement<decimal>> dynamoResult, 
            Dictionary<string, HashSet<SnapshotItem>> output,
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
                    itemsOnDate = new HashSet<SnapshotItem> {new SnapshotItem {Description = label, Value = r.Value}};
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
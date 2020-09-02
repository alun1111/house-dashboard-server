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
        private readonly IFormatProvider _culture 
            = CultureInfo.CreateSpecificCulture("en-GB");

        private readonly RainfallReadingsRepository _rainfallReadingsRepository;
        private readonly RiverLevelReadingsRepository _riverLevelsRepository;

        public SnapshotRangeRangeFactory()
        {
            _riverLevelsRepository = new RiverLevelReadingsRepository();
            _rainfallReadingsRepository = new RainfallReadingsRepository();
        }

        public Dictionary<string, HashSet<SnapshotItem>> Build()
        {
            var output = new Dictionary<string, HashSet<SnapshotItem>>();
            
            Task<List<IDynamoDbItem<decimal>>> rainfallLevels 
                = _rainfallReadingsRepository.GetReadingItems("14881");
            
            Task<List<IDynamoDbItem<decimal>>> riverLevels 
                = _riverLevelsRepository.GetReadingItems("14881-SG");

            Task.WaitAll(rainfallLevels, riverLevels);

            TryAddSnapshotItem(rainfallLevels.Result, output, "Whitburn - Rainfall");
            TryAddSnapshotItem(riverLevels.Result, output, "Whitburn - RiverLevel");
            
            return output;
        }

        private void TryAddSnapshotItem(List<IDynamoDbItem<decimal>> dynamoResult, 
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

        private static string RoundedTime(IDynamoDbItem<decimal> r)
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
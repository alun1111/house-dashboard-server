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

        public SnapshotRangeRangeFactory()
        {
            _rainfallReadingsRepository = new RainfallReadingsRepository();
        }

        public IEnumerable<Snapshot> Build()
        {
            Task<List<IDynamoDbItem<decimal>>> readingItems 
                = _rainfallReadingsRepository.GetReadingItems("14881");

            foreach (var r in readingItems.Result)
            {
                yield return new Snapshot()
                {
                    Moment = r.MeasurementTime,
                    Rainfall = new SnapshotItem()
                    {
                        Description = "Whitburn",
                        Value = r.Value
                    },
                    RiverLevel = new SnapshotItem()
                    {
                        Description = "Whitburn",
                        Value = 1.5M
                    }
                };
            }
        }
    }
}
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using house_dashboard_server.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace house_dashboard_server.Data
{
    public class RiverLevelReadingsRepository
    {
        private readonly IFormatProvider _culture 
            = CultureInfo.CreateSpecificCulture("en-GB");

        private readonly DynamoTableQueryRunner _dynamoTableQueryRunner;
        private readonly NumberReadingFactory _numberReadingFactory;

        public RiverLevelReadingsRepository()
        {
            _dynamoTableQueryRunner = new DynamoTableQueryRunner();
            _numberReadingFactory = new NumberReadingFactory();
        }

        public async Task<ReadingSet<decimal>> GetReadingSet(string stationId)
        {
            using var client = new AmazonDynamoDBClient(RegionEndpoint.EUWest1);

            var queryResult = await
                _dynamoTableQueryRunner.QueryDynamoDbTable(client,
                    "river-level-readings",
                    "monitoring-station-id",
                    stationId);

            return new ReadingSet<decimal>()
            {
                Readings = new List<Reading<decimal>>()
                { 
                    PrepareRiverLevelReading(queryResult, stationId),
                }
            };
        }

        private Reading<decimal> PrepareRiverLevelReading(List<Document> queryResult, string stationId)
        {
            var reducedScanResult = new List<DynamoDbItem<decimal>>();

            queryResult.ForEach((d) =>
            {
                var depth = decimal.Parse(d["depth"], _culture);
                reducedScanResult.Add(new DynamoDbItem<decimal>(
                    DateTime.Parse(d["timestamp"], _culture),
                    depth
                    ));
            });

            return _numberReadingFactory.BuildReading(stationId, reducedScanResult);
        }
    }
}

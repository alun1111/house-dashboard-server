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
        private const string STATION_ID_ALMONDELL = "14869-SG";
        private const string STATION_NAME_ALMONDELL = "Almond (Lothian) : Almondell";

        private readonly IFormatProvider _culture 
            = CultureInfo.CreateSpecificCulture("en-GB");

        private readonly DynamoTableQueryRunner _dynamoTableQueryRunner;
        private readonly NumberReadingFactory _numberReadingFactory;

        public RiverLevelReadingsRepository()
        {
            _dynamoTableQueryRunner = new DynamoTableQueryRunner();
            _numberReadingFactory = new NumberReadingFactory();
        }

        public async Task<ReadingSet> GetReadingSet()
        {
            using var client = new AmazonDynamoDBClient(RegionEndpoint.EUWest1);

            var queryResult = await
                _dynamoTableQueryRunner.QueryDynamoDbTable(client,
                    "river-level-readings",
                    "monitoring-station-id",
                    STATION_ID_ALMONDELL);

            return new ReadingSet()
            {
                Readings = new List<Reading<decimal>>()
                { 
                    PrepareRiverLevelReading(queryResult),
                }
            };
        }

        private Reading<decimal> PrepareRiverLevelReading(List<Document> queryResult)
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

            return _numberReadingFactory.BuildReading(STATION_NAME_ALMONDELL, reducedScanResult);
        }
    }
}

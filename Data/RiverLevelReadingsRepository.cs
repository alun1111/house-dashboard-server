using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using HouseDashboardServer.Models;

namespace HouseDashboardServer.Data
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

        public Task<NumberReading<decimal>> GetReading(string stationId)
        {
            using var client = new AmazonDynamoDBClient(RegionEndpoint.EUWest1);

            var queryResult = 
                _dynamoTableQueryRunner.QueryOnTimestampRange(client,
                    tableName: "river-level-readings",
                    partionKey: "monitoring-station-id",
                    partitionValue: stationId,
                    days: 3);

            return PrepareRiverLevelReading(queryResult.Result, stationId);
        }

        private Task<NumberReading<decimal>> PrepareRiverLevelReading(List<Document> queryResult, string stationId)
        {
            var reducedScanResult = new List<DynamoDbItem<decimal>>();

            queryResult.ForEach((d) =>
            {
                var readingDate = DateTime.Parse(d["timestamp"], _culture);
                var dateTimeOffset = new DateTimeOffset(readingDate);
                var unixDateTime = dateTimeOffset.ToUnixTimeSeconds();
                var depth = decimal.Parse(d["depth"], _culture);
                
                reducedScanResult.Add(new DynamoDbItem<decimal>(
                    readingDate, unixDateTime,depth
                    ));
            });

            return _numberReadingFactory.BuildReading(stationId, reducedScanResult);
        }
    }
}

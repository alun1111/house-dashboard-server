using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using house_dashboard_server.Models;

namespace house_dashboard_server.Data
{
    public class RainfallReadingsRepository
    {
        private readonly IFormatProvider _culture 
            = CultureInfo.CreateSpecificCulture("en-GB");

        private readonly DynamoTableQueryRunner _dynamoTableQueryRunner;
        private readonly NumberReadingFactory _numberReadingFactory;

        public RainfallReadingsRepository()
        {
            _dynamoTableQueryRunner = new DynamoTableQueryRunner();
            _numberReadingFactory = new NumberReadingFactory();
        }

        public async Task<ReadingSet<decimal>> GetReadingSet()
        {
            throw new NotImplementedException("No requirement for returning all at once yet");
        }

        public async Task<NumberReading<decimal>> GetReading(string stationId)
        {
            using var client = new AmazonDynamoDBClient(RegionEndpoint.EUWest1);

            var queryResult = await
                _dynamoTableQueryRunner.QueryOnTimestampRange(client,
                    tableName: "rainfall-readings",
                    partionKey: "monitoring-station-id",
                    partitionValue: stationId,
                    days: 3);

            return PrepareRiverLevelReading(queryResult, stationId);
        }

        private NumberReading<decimal> PrepareRiverLevelReading(List<Document> queryResult, string stationId)
        {
            var reducedScanResult = new List<DynamoDbItem<decimal>>();

            queryResult.ForEach((d) =>
            {
                var depth = decimal.Parse(d["amount"], _culture);
                var readingDate = DateTime.Parse(d["timestamp"], _culture);
                var dateTimeOffset = new DateTimeOffset(readingDate);
                var unixDateTime = dateTimeOffset.ToUnixTimeSeconds();
                
                reducedScanResult.Add(new DynamoDbItem<decimal>(
                    readingDate, unixDateTime, depth
                    ));
            });

            return _numberReadingFactory.BuildReading(stationId, reducedScanResult);
        }
    }
}
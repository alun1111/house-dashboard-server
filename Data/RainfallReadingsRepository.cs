using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using HouseDashboardServer.Models;

namespace HouseDashboardServer.Data
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

        public Task<NumberReading<decimal>> GetReading(string stationId)
        {
            return GetReading(stationId, DateTime.Today);
        }

        public Task<NumberReading<decimal>> GetReading(string stationId, DateTime dateFrom)
        {
            using var client = new AmazonDynamoDBClient(RegionEndpoint.EUWest1);
            
            var days = 3;

            if (dateFrom != DateTime.Today)
            {
                var ts = dateFrom.Subtract(DateTime.Today);
                days = Math.Abs(ts.Days); // fromDate in future be damned
            }

            var queryResult = 
                _dynamoTableQueryRunner.QueryOnTimestampRange(client,
                    tableName: "rainfall-readings",
                    partionKey: "monitoring-station-id",
                    partitionValue: stationId,
                    days: days);

            return PrepareRainfallReading(queryResult, stationId);
        }

        private async Task<NumberReading<decimal>> PrepareRainfallReading(Task<List<Document>> queryResult, string stationId)
        {
            var reducedScanResult = new List<DynamoDbItem<decimal>>();

            foreach (var d in await queryResult)
            {
                var depth = decimal.Parse(d["amount"], _culture);
                var readingDate = DateTime.Parse(d["timestamp"], _culture);
                var dateTimeOffset = new DateTimeOffset(readingDate);
                var unixDateTime = dateTimeOffset.ToUnixTimeSeconds();
                
                reducedScanResult.Add(new DynamoDbItem<decimal>(
                    readingDate, unixDateTime, depth
                    ));
            }

            return _numberReadingFactory.BuildReading(stationId, reducedScanResult);
        }
    }
}
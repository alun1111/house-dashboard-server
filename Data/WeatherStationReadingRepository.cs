using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using house_dashboard_server.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace house_dashboard_server.Data
{
    public class WeatherStationReadingRepository
    {
        private readonly IFormatProvider _culture 
            = CultureInfo.CreateSpecificCulture("en-GB");

        public async Task<ReadingSet> GetReadingSet()
        {
            using var client = new AmazonDynamoDBClient(RegionEndpoint.EUWest1);

            var queryResult = await
                QueryDynamoDbTable(client,
                    "weather-station-readings",
                    "station-id",
                    "wmr-89");

            return new ReadingSet()
            {
                Readings = new List<Reading<decimal>>()
                { 
                    PrepareOutsideTempReading(queryResult),
                    PrepareInsideTempReading(queryResult)
                }
            };
        }

        private Reading<decimal> PrepareOutsideTempReading(List<Document> queryResult)
        {
            var reducedScanResult = new List<DynamoDbItem<decimal>>();

            queryResult.ForEach((d) =>
            {
                var temp = decimal.Parse(d["outside-temp"], _culture);
                var convertedTemp = (temp - 32) * 5 / 9;
                reducedScanResult.Add(new DynamoDbItem<decimal>(
                    DateTime.Parse(d["timestamp"], _culture),
                    convertedTemp
                    ));
            });

            return BuildReading("OutsideTemperature", reducedScanResult);
        }

        private Reading<decimal> PrepareInsideTempReading(List<Document> queryResult)
        {
            List<DynamoDbItem<decimal>> reducedScanResult = new List<DynamoDbItem<decimal>>();

            queryResult.ForEach((d) =>
            {
                var temp = decimal.Parse(d["inside-temp"], _culture);
                var convertedTemp = (temp - 32) * 5 / 9;
                reducedScanResult.Add(new DynamoDbItem<decimal>(
                    DateTime.Parse(d["timestamp"], _culture),
                    convertedTemp
                    ));
            });

            return BuildReading("InsideTemperature", reducedScanResult);
        }

        private static Reading<decimal> BuildReading(string measurementName, 
            List<DynamoDbItem<decimal>> reducedScanResult)
        {
            var orderedScanResult = reducedScanResult.OrderBy(x => x.MeasurementTime);
            var latestMeasurement = orderedScanResult.LastOrDefault();

            return new Reading<decimal>(
                name: measurementName,
                current: new DynamoDbItem<decimal>(
                    latestMeasurement.MeasurementTime,
                    latestMeasurement.Value),
                recent: orderedScanResult
                    .Skip(1)
                    .Select(s => new DynamoDbItem<decimal>(s.MeasurementTime, s.Value) as IDynamoDbItem<decimal>)
                    .ToList()
                    );
        }

        private static async Task<List<Document>> QueryDynamoDbTable(AmazonDynamoDBClient client,
                                                                     string tableName,
                                                                     string partionKey,
                                                                     string partitionValue)
        {
            var table = Table.LoadTable(client, tableName);

            var queryFilter = 
                new QueryFilter(partionKey, QueryOperator.Equal, partitionValue);

            queryFilter.AddCondition("timestamp",
                QueryOperator.GreaterThan,
                DateTime.UtcNow.AddDays(-1));

            var queryResult = await table
                .Query(queryFilter)
                .GetRemainingAsync()
                .ConfigureAwait(false);

            return queryResult;
        }
    }
}

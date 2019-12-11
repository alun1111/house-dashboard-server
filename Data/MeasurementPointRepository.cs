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
    public class MeasurementPointRepository
    {
        private readonly IFormatProvider _culture = CultureInfo.CreateSpecificCulture("en-GB");

        public MeasurementPoint GetMeasurementPoint()
        {
            using var client = new AmazonDynamoDBClient(RegionEndpoint.EUWest1);

            var outsideTemperatureMeasurement = GetWeatherMeasurement(client);
            var riverLevelMeasurement = GetRiverLevelsMeasurement(client);

            Task.WaitAll(outsideTemperatureMeasurement, riverLevelMeasurement);

            return new MeasurementPoint()
            {
                ReportingTime = DateTime.Now,
                Measurements = new List<Measurement<decimal>>()
                    { 
                        outsideTemperatureMeasurement.Result,
                        riverLevelMeasurement.Result
                    }
            };
        }

        private async Task<Measurement<decimal>> GetWeatherMeasurement(AmazonDynamoDBClient client)
        {
            List<Document> weatherTableScanResult = 
                await GetRecentTableItems(client, "Weather").ConfigureAwait(false);

            List<DynamoDbItem<decimal>> reducedScanResult = new List<DynamoDbItem<decimal>>();

            weatherTableScanResult.ForEach((d) =>
            {
                var temp = decimal.Parse(d["OutsideTemperature"], _culture);
                var convertedTemp = (temp - 32) * 5 / 9;
                    reducedScanResult.Add(new DynamoDbItem<decimal>(
                        DateTime.Parse(d["MeasurementTime"], _culture),
                        convertedTemp
                        ));
            });

            return CreateMeasurement("OutsideTemperature", reducedScanResult);
        }

        private async Task<Measurement<decimal>> GetRiverLevelsMeasurement(AmazonDynamoDBClient client)
        {
            List<Document> riverLevelTableScanResult =
                await GetRecentTableItems(client, "RiverLevels").ConfigureAwait(false);

            List<DynamoDbItem<decimal>> reducedScanResult = new List<DynamoDbItem<decimal>>();

            riverLevelTableScanResult.ForEach((d) =>
            {
                reducedScanResult.Add(new DynamoDbItem<decimal>(
                    DateTime.Parse(d["MeasurementTime"], _culture),
                    decimal.Parse(d["WaterLevel"], _culture)));
            });

            return CreateMeasurement("RiverLevel", reducedScanResult);
        }

        private static Measurement<decimal> CreateMeasurement(string measurementName, 
            List<DynamoDbItem<decimal>> reducedScanResult)
        {
            var orderedScanResult = reducedScanResult.OrderByDescending(x => x.MeasurementTime);
            var latestMeasurement = orderedScanResult.FirstOrDefault();

            return new Measurement<decimal>(
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

        private static async Task<List<Document>> GetRecentTableItems(AmazonDynamoDBClient client, string tableName)
        {
            var table = Table.LoadTable(client, tableName);

            ScanFilter scanFilter = new ScanFilter();
            scanFilter.AddCondition("MeasurementTime",
                ScanOperator.GreaterThan,
                DateTime.UtcNow.AddDays(-1));

            var resultsList = await table
                .Scan(scanFilter)
                .GetRemainingAsync()
                .ConfigureAwait(false);
            return resultsList;
        }
    }
}

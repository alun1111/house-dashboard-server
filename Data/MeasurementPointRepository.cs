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

        public async Task<MeasurementPoint> GetMeasurementPoint()
        {
            using var client = new AmazonDynamoDBClient(RegionEndpoint.EUWest1);
            return new MeasurementPoint()
            {
                MeasurementTime = DateTime.Now,
                Measurements = new List<Measurement>()
                    {
                        await GetWeatherMeasurement(client).ConfigureAwait(false)
                    }
            };
        }

        private async Task<Measurement> GetWeatherMeasurement(AmazonDynamoDBClient client)
        {
            var table = Table.LoadTable(client, "Weather");

            ScanFilter scanFilter = new ScanFilter();
            scanFilter.AddCondition("MeasurementTime", 
                ScanOperator.GreaterThan, 
                DateTime.UtcNow.AddDays(-1));

            var resultsList = await table
                .Scan(scanFilter)
                .GetRemainingAsync()
                .ConfigureAwait(false);

            List<TemperatureItem> items = new List<TemperatureItem>();

            resultsList.ForEach((d) =>
            {
                items.Add(new TemperatureItem(
                    DateTime.Parse(d["MeasurementTime"], _culture), 
                    decimal.Parse(d["OutsideTemperature"], _culture)));
            });

            var orderedItems = items.OrderByDescending(x => x.MeasurementTime);
            var current = orderedItems.FirstOrDefault();

            return new Measurement(
                name: "CurrentTemperature",
                current: new TemperatureItem(current.MeasurementTime, current.Value),
                recent: orderedItems
                    .Skip(1)
                    .Select(s => new TemperatureItem(s.MeasurementTime, s.Value) as IDynamoDbItem)
                    .ToList()
                    );
        }
    }
}

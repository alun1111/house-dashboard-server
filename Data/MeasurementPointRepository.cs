using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using house_dashboard_server.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace house_dashboard_server.Data
{
    public class MeasurementPointRepository
    {
        public MeasurementPoint GetMeasurementPoint()
        {
            var client = new AmazonDynamoDBClient(RegionEndpoint.EUWest1);
            
            return new MeasurementPoint()
            {
                MeasurementTime = DateTime.Now,
                Measurements = new List<Measurement>()
                {
                    GetWeatherMeasurement(client)
                }
            };

        }

        private Measurement GetWeatherMeasurement(AmazonDynamoDBClient client)
        {
            var table = Table.LoadTable(client, "Weather");

            ScanFilter scanFilter = new ScanFilter();
            scanFilter.AddCondition("MeasurementTime", 
                ScanOperator.GreaterThan, 
                DateTime.UtcNow.AddDays(-1));

            var resultsList = table
                .Scan(scanFilter)
                .GetRemainingAsync()
                .Result;

            List<TemperatureItem> items = new List<TemperatureItem>();

            resultsList.ForEach((d) =>
            {
                items.Add(new TemperatureItem(
                    DateTime.Parse(d["MeasurementTime"]), 
                    decimal.Parse(d["OutsideTemperature"])));
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

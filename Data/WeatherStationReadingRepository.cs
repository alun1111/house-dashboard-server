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
    public class WeatherStationReadingRepository
    {
        private readonly IFormatProvider _culture 
            = CultureInfo.CreateSpecificCulture("en-GB");

        private readonly DynamoTableQueryRunner _dynamoTableQueryRunner;
        private readonly NumberReadingFactory _numberReadingFactory;

        public WeatherStationReadingRepository()
        {
            _dynamoTableQueryRunner = new DynamoTableQueryRunner();
            _numberReadingFactory = new NumberReadingFactory();
        }

        public async Task<ReadingSet<decimal>> GetReadingSet()
        {
            using var client = new AmazonDynamoDBClient(RegionEndpoint.EUWest1);

            var queryResult = await
                _dynamoTableQueryRunner.QueryDynamoDbTable(client,
                    "weather-station-readings",
                    "station-id",
                    "wmr-89");

            return new ReadingSet<decimal>()
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

            return _numberReadingFactory.BuildReading("OutsideTemperature", reducedScanResult);
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

            return _numberReadingFactory.BuildReading("InsideTemperature", reducedScanResult);
        }
    }
}

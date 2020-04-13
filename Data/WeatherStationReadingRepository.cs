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
            throw new NotImplementedException();
        }

        public async Task<NumberReading<decimal>> GetTemperatureReading(string stationId, TemperatureReadingType type)
        {
            using var client = new AmazonDynamoDBClient(RegionEndpoint.EUWest1);

            var queryResult = await
                _dynamoTableQueryRunner.QueryOnTimestampRange(client,
                    tableName: "weather-station-readings",
                    partionKey: "station-id",
                    partitionValue: stationId,
                    days: 1);

            switch (type)
            {
                case TemperatureReadingType.INSIDE:
                    return PrepareInsideTempReading(queryResult);
                case TemperatureReadingType.OUTSIDE:
                    return PrepareOutsideTempReading(queryResult);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown temperature reading type");
            }
        }

        private NumberReading<decimal> PrepareOutsideTempReading(List<Document> queryResult)
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

        private NumberReading<decimal> PrepareInsideTempReading(List<Document> queryResult)
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

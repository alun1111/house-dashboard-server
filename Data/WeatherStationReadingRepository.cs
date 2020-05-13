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

        public Task<NumberReading<decimal>> GetTemperatureReading(string stationId, TemperatureReadingType type)
        {
            using var client = new AmazonDynamoDBClient(RegionEndpoint.EUWest1);
            
            var queryResult = 
                _dynamoTableQueryRunner.QueryOnTimestampRange(client,
                    tableName: "weather-station-readings",
                    partionKey: "station-id",
                    partitionValue: stationId,
                    days: 1);

            switch (type)
            {
                case TemperatureReadingType.INSIDE:
                    return PrepareInsideTempReading(queryResult.Result);
                case TemperatureReadingType.OUTSIDE:
                    return PrepareOutsideTempReading(queryResult.Result);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown temperature reading type");
            }
        }

        private Task<NumberReading<decimal>> PrepareOutsideTempReading(List<Document> queryResult)
        {
            var reducedScanResult = new List<DynamoDbItem<decimal>>();

            queryResult.ForEach((d) =>
            {
                var temp = decimal.Parse(d["outside-temp"], _culture);
                var convertedTemp = (temp - 32) * 5 / 9;
                var readingDate = DateTime.Parse(d["timestamp"], _culture);
                var dateTimeOffset = new DateTimeOffset(readingDate);
                var unixDateTime = dateTimeOffset.ToUnixTimeSeconds();
                
                reducedScanResult.Add(new DynamoDbItem<decimal>(
                    readingDate, unixDateTime,convertedTemp
                    ));
            });

            return _numberReadingFactory.BuildReading("OutsideTemperature", reducedScanResult);
        }

        private Task<NumberReading<decimal>> PrepareInsideTempReading(List<Document> queryResult)
        {
            List<DynamoDbItem<decimal>> reducedScanResult = new List<DynamoDbItem<decimal>>();

            queryResult.ForEach((d) =>
            {
                var temp = decimal.Parse(d["inside-temp"], _culture);
                var convertedTemp = (temp - 32) * 5 / 9;
                var readingDate = DateTime.Parse(d["timestamp"], _culture);
                var dateTimeOffset = new DateTimeOffset(readingDate);
                var unixDateTime = dateTimeOffset.ToUnixTimeSeconds();
                
                reducedScanResult.Add(new DynamoDbItem<decimal>(
                    readingDate, unixDateTime,convertedTemp
                    ));
            });

            return _numberReadingFactory.BuildReading("InsideTemperature", reducedScanResult);
        }
    }
}

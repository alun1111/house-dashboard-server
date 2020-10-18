using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;
using house_dashboard_server.Data.Interfaces;
using house_dashboard_server.Models;

namespace house_dashboard_server.Data
{
    public class WeatherStationReadingRepository : IWeatherStationReadingRepository
    {
        private readonly IDynamoTableQueryRunner _dynamoTableQueryRunner;

        private readonly IFormatProvider _culture 
            = CultureInfo.CreateSpecificCulture("en-GB");

        public WeatherStationReadingRepository(IDynamoTableQueryRunner dynamoTableQueryRunner)
        {
            _dynamoTableQueryRunner = dynamoTableQueryRunner;
        }

        public Task<Reading<decimal>> GetTemperatureReading(string stationId, TemperatureReadingType type)
        {
            
            var queryResult = 
                _dynamoTableQueryRunner.QueryOnTimestampRange(
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

        private async Task<Reading<decimal>> PrepareOutsideTempReading(Task<List<Document>> queryResult)
        {
            var reducedScanResult = new List<IMeasurement<decimal>>();

            foreach (var d in await queryResult)
            {
                var temp = decimal.Parse(d["outside-temp"], _culture);
                var convertedTemp = (temp - 32) * 5 / 9;
                var readingDate = DateTime.Parse(d["timestamp"], _culture);
                var dateTimeOffset = new DateTimeOffset(readingDate);
                var unixDateTime = dateTimeOffset.ToUnixTimeSeconds();
                
                reducedScanResult.Add(new Measurement<decimal>(
                    readingDate, unixDateTime,convertedTemp
                    ));
            }

            return ReadingFactory.BuildReading("OutsideTemperature", reducedScanResult);
        }

        private async Task<Reading<decimal>> PrepareInsideTempReading(Task<List<Document>> queryResult)
        {
            List<IMeasurement<decimal>> reducedScanResult = new List<IMeasurement<decimal>>();

            foreach (var d in await queryResult)
            {
                var temp = decimal.Parse(d["inside-temp"], _culture);
                var convertedTemp = (temp - 32) * 5 / 9;
                var readingDate = DateTime.Parse(d["timestamp"], _culture);
                var dateTimeOffset = new DateTimeOffset(readingDate);
                var unixDateTime = dateTimeOffset.ToUnixTimeSeconds();
                
                reducedScanResult.Add(new Measurement<decimal>(
                    readingDate, unixDateTime,convertedTemp
                    ));
            }

            return ReadingFactory.BuildReading("InsideTemperature", reducedScanResult);
        }
    }
}

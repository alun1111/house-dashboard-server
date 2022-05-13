using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;
using house_dashboard_server.Calculators;
using house_dashboard_server.Data.Models;
using Microsoft.Extensions.Logging;

namespace house_dashboard_server.Data.DynamoDB
{
    public class RiverLevelReadingsRepository : IRiverLevelReadingsRepository
    {
        private readonly IDynamoTableQueryRunner _dynamoTableQueryRunner;

        private readonly IFormatProvider _culture 
            = CultureInfo.CreateSpecificCulture("en-GB");

        private readonly ILogger<RiverLevelReadingsRepository> _logger;


        public RiverLevelReadingsRepository(IDynamoTableQueryRunner dynamoTableQueryRunner
            , ILogger<RiverLevelReadingsRepository> logger)
        {
            _logger = logger;
            _dynamoTableQueryRunner = dynamoTableQueryRunner;
        }
        
        public Task<Reading<decimal>> GetReading(string stationId, DateTime dateFrom = default)
        {
            var queryResult = 
                _dynamoTableQueryRunner.QueryOnTimestampRange(
                    tableName: "river-level-readings",
                    partionKey: "monitoring-station-id",
                    partitionValue: stationId,
                    days: DaysCalculator.DaysSinceDateFrom(dateFrom));

            return PrepareRiverLevelReading(queryResult, stationId);
        }

        public Task<List<IMeasurement<decimal>>> GetReadingItems(string stationId, DateTime dateFrom = default)
        {
            _logger.Log(LogLevel.Debug, "Start: " + ExactTimeToString() + ", RiverLevels GetReading for: " + stationId);
            
            var queryResult =
                _dynamoTableQueryRunner.QueryOnTimestampRange(
                    tableName: "river-level-readings",
                    partionKey: "monitoring-station-id",
                    partitionValue: stationId,
                    days: DaysCalculator.DaysSinceDateFrom(dateFrom));

            _logger.Log(LogLevel.Debug, "End: " + ExactTimeToString() + ", RiverLevels GetReading for: " + stationId);
            
            return GetReducedScanResult(queryResult); 
        }

        private string ExactTimeToString()
        {
            return DateTime.Now.ToString("hh:mm:ss.fff", _culture);
        }

        private async Task<Reading<decimal>> PrepareRiverLevelReading(Task<List<Document>> queryResult, string stationId)
        {
            var reducedScanResult = await GetReducedScanResult(queryResult); 

            return ReadingFactory.BuildReading(stationId, reducedScanResult);
        }
        
        private async Task<List<IMeasurement<decimal>>> GetReducedScanResult(Task<List<Document>> queryResult)
        {
            var reducedScanResult = new List<IMeasurement<decimal>>();
            
            foreach (var d in await queryResult)
            {
                var depth = decimal.Parse(d["depth"], _culture);
                var readingDate = DateTime.Parse(d["timestamp"], _culture);
                var dateTimeOffset = new DateTimeOffset(readingDate);
                var unixDateTime = dateTimeOffset.ToUnixTimeSeconds();

                reducedScanResult.Add(new Measurement<decimal>(
                    readingDate, unixDateTime, depth
                ));
            }

            return reducedScanResult;
        }
    }
}

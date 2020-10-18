using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;
using house_dashboard_server.Calculators;
using house_dashboard_server.Data.Interfaces;
using house_dashboard_server.Models;
using Microsoft.Extensions.Logging;

namespace house_dashboard_server.Data
{
    public class RainfallReadingsRepository : IRainfallReadingsRepository
    {
        private readonly ILogger<RainfallReadingsRepository> _logger;
        private readonly IDynamoTableQueryRunner _dynamoTableQueryRunner;

        private readonly IFormatProvider _culture
            = CultureInfo.CreateSpecificCulture("en-GB");

        public RainfallReadingsRepository(ILogger<RainfallReadingsRepository> logger,
            IDynamoTableQueryRunner dynamoTableQueryRunner)
        {
            _logger = logger;
            _dynamoTableQueryRunner = dynamoTableQueryRunner;
        }

        public Task<Reading<decimal>> GetReading(string stationId, DateTime dateFrom = default)
        {
            var queryResult =
                _dynamoTableQueryRunner.QueryOnTimestampRange(
                    tableName: "rainfall-readings",
                    partionKey: "monitoring-station-id",
                    partitionValue: stationId,
                    days: DaysCalculator.DaysSinceDateFrom(dateFrom));

            return PrepareRainfallReading(queryResult, stationId);
        }
        
        public Task<List<IMeasurement<decimal>>> GetMeasurements(string stationId, DateTime dateFrom = default)
        {
            var queryResult =
                _dynamoTableQueryRunner.QueryOnTimestampRange(
                    tableName: "rainfall-readings",
                    partionKey: "monitoring-station-id",
                    partitionValue: stationId,
                    days: DaysCalculator.DaysSinceDateFrom(dateFrom));

            return GetReducedScanResult(queryResult); 
        }


        private async Task<Reading<decimal>> PrepareRainfallReading(Task<List<Document>> queryResult,
            string stationId)
        {
            var reducedScanResult = await GetReducedScanResult(queryResult); 

            return ReadingFactory.BuildReading(stationId, reducedScanResult);
        }

        private async Task<List<IMeasurement<decimal>>> GetReducedScanResult(Task<List<Document>> queryResult)
        {
            var reducedScanResult = new List<IMeasurement<decimal>>();
            
            _logger.Log(LogLevel.Debug, "Starting querying rainfall data");

            foreach (var d in await queryResult)
            {
                var depth = decimal.Parse(d["amount"], _culture);
                var readingDate = DateTime.Parse(d["timestamp"], _culture);
                var dateTimeOffset = new DateTimeOffset(readingDate);
                var unixDateTime = dateTimeOffset.ToUnixTimeSeconds();

                reducedScanResult.Add(new Measurement<decimal>(
                    readingDate, unixDateTime, depth
                ));
            }
            
            _logger.Log(LogLevel.Debug, "Finished querying rainfall data");

            return reducedScanResult;
        }
    }
}
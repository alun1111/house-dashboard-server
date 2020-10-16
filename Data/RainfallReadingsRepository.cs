using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using HouseDashboardServer.Models;
using HouseDashboardServer.Utils;
using Microsoft.Extensions.Logging;

namespace HouseDashboardServer.Data
{
    public class RainfallReadingsRepository : IRainfallReadingsRepository
    {
        private readonly ILogger<RainfallReadingsRepository> _logger;

        private readonly IFormatProvider _culture
            = CultureInfo.CreateSpecificCulture("en-GB");

        private readonly DynamoTableQueryRunner _dynamoTableQueryRunner;
        private readonly NumberReadingFactory _numberReadingFactory;

        public RainfallReadingsRepository(ILogger<RainfallReadingsRepository> logger)
        {
            _logger = logger;
            _dynamoTableQueryRunner = new DynamoTableQueryRunner();
            _numberReadingFactory = new NumberReadingFactory();
        }

        public Task<NumberReading<decimal>> GetReading(string stationId, DateTime dateFrom = default)
        {
            using var client = new AmazonDynamoDBClient(RegionEndpoint.EUWest1);
            
            var queryResult =
                _dynamoTableQueryRunner.QueryOnTimestampRange(client,
                    tableName: "rainfall-readings",
                    partionKey: "monitoring-station-id",
                    partitionValue: stationId,
                    days: DaysCalculator.DaysSinceDateFrom(dateFrom));

            return PrepareRainfallReading(queryResult, stationId);
        }
        
        public Task<List<IDynamoDbItem<decimal>>> GetReadingItems(string stationId, DateTime dateFrom = default)
        {
            using var client = new AmazonDynamoDBClient(RegionEndpoint.EUWest1);

            var queryResult =
                _dynamoTableQueryRunner.QueryOnTimestampRange(client,
                    tableName: "rainfall-readings",
                    partionKey: "monitoring-station-id",
                    partitionValue: stationId,
                    days: DaysCalculator.DaysSinceDateFrom(dateFrom));

            return GetReducedScanResult(queryResult); 
        }


        private async Task<NumberReading<decimal>> PrepareRainfallReading(Task<List<Document>> queryResult,
            string stationId)
        {
            var reducedScanResult = await GetReducedScanResult(queryResult); 

            return _numberReadingFactory.BuildReading(stationId, reducedScanResult);
        }

        private async Task<List<IDynamoDbItem<decimal>>> GetReducedScanResult(Task<List<Document>> queryResult)
        {
            var reducedScanResult = new List<IDynamoDbItem<decimal>>();
            
            _logger.Log(LogLevel.Debug, "Starting querying rainfall data");

            foreach (var d in await queryResult)
            {
                var depth = decimal.Parse(d["amount"], _culture);
                var readingDate = DateTime.Parse(d["timestamp"], _culture);
                var dateTimeOffset = new DateTimeOffset(readingDate);
                var unixDateTime = dateTimeOffset.ToUnixTimeSeconds();

                reducedScanResult.Add(new DynamoDbItem<decimal>(
                    readingDate, unixDateTime, depth
                ));
            }
            
            _logger.Log(LogLevel.Debug, "Finished querying rainfall data");

            return reducedScanResult;
        }
    }
}
﻿using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using HouseDashboardServer.Models;
using HouseDashboardServer.Utils;

namespace HouseDashboardServer.Data
{
    public class RiverLevelReadingsRepository
    {
        private readonly IFormatProvider _culture 
            = CultureInfo.CreateSpecificCulture("en-GB");

        private readonly DynamoTableQueryRunner _dynamoTableQueryRunner;
        private readonly NumberReadingFactory _numberReadingFactory;

        public RiverLevelReadingsRepository()
        {
            _dynamoTableQueryRunner = new DynamoTableQueryRunner();
            _numberReadingFactory = new NumberReadingFactory();
        }
        
        public Task<NumberReading<decimal>> GetReading(string stationId, DateTime dateFrom)
        {
            using var client = new AmazonDynamoDBClient(RegionEndpoint.EUWest1);

            var queryResult = 
                _dynamoTableQueryRunner.QueryOnTimestampRange(client,
                    tableName: "river-level-readings",
                    partionKey: "monitoring-station-id",
                    partitionValue: stationId,
                    days: DaysCalculator.DaysSinceDateFrom(dateFrom));

            return PrepareRiverLevelReading(queryResult, stationId);
        }

        private async Task<NumberReading<decimal>> PrepareRiverLevelReading(Task<List<Document>> queryResult, string stationId)
        {
            var reducedScanResult = new List<IDynamoDbItem<decimal>>();
            
            foreach (var d in await queryResult)
            {
                var readingDate = DateTime.Parse(d["timestamp"], _culture);
                var dateTimeOffset = new DateTimeOffset(readingDate);
                var unixDateTime = dateTimeOffset.ToUnixTimeSeconds();
                var depth = decimal.Parse(d["depth"], _culture);
                
                reducedScanResult.Add(new DynamoDbItem<decimal>(
                    readingDate, unixDateTime,depth
                    ));
            }

            return _numberReadingFactory.BuildReading(stationId, reducedScanResult);
        }
    }
}

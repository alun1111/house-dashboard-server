using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HouseDashboardServer.Models;

namespace HouseDashboardServer.Data
{
    public interface IRainfallReadingsRepository
    {
        Task<NumberReading<decimal>> GetReading(string stationId, DateTime dateFrom = default);
        Task<List<IDynamoDbItem<decimal>>> GetReadingItems(string stationId, DateTime dateFrom = default);
    }
}
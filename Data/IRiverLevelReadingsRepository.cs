using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HouseDashboardServer.Models;

namespace HouseDashboardServer.Data
{
    public interface IRiverLevelReadingsRepository
    {
        Task<Reading<decimal>> GetReading(string stationId, DateTime dateFrom = default);
        Task<List<IMeasurement<decimal>>> GetReadingItems(string stationId, DateTime dateFrom = default);
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using house_dashboard_server.Models;

namespace house_dashboard_server.Data.Interfaces
{
    public interface IRainfallReadingsRepository
    {
        Task<Reading<decimal>> GetReading(string stationId, DateTime dateFrom = default);
        Task<List<IMeasurement<decimal>>> GetMeasurements(string stationId, DateTime dateFrom = default);
    }
}
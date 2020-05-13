using System;
using System.Threading.Tasks;
using HouseDashboardServer.Models;

namespace HouseDashboardServer.Factories
{
    public class RainfallSummaryFactory : ISummaryFactory<RainfallSummary>
    {
        public Task<RainfallSummary> Build()
        {
            throw new NotImplementedException();
        }
    }
}
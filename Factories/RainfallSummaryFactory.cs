using System;
using System.Threading.Tasks;
using house_dashboard_server.Models;

namespace house_dashboard_server.Factories
{
    public class RainfallSummaryFactory : ISummaryFactory<RainfallSummary>
    {
        public Task<RainfallSummary> Build()
        {
            throw new NotImplementedException();
        }
    }
}
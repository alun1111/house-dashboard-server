using System.Threading.Tasks;
using HouseDashboardServer.Models;

namespace HouseDashboardServer.Factories
{
    public interface ISummaryFactory<T>
    {
        public Task<RainfallSummary> Build();
    }
}
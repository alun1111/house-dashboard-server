using System.Threading.Tasks;
using house_dashboard_server.Models;

namespace house_dashboard_server.Factories
{
    public interface ISummaryFactory<T>
    {
        public Task<RainfallSummary> Build();
    }
}
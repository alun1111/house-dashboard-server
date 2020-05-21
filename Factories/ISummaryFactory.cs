using System.Threading.Tasks;
using HouseDashboardServer.Models;

namespace HouseDashboardServer.Factories
{
    public interface ISummaryFactory<T>
    {
        public Summary Build(string id);
    }
}
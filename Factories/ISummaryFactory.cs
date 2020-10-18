using house_dashboard_server.Models;

namespace house_dashboard_server.Factories
{
    public interface ISummaryFactory<T>
    {
        public Summary Build();
    }
}
using System;
using house_dashboard_server.Data.Models;

namespace house_dashboard_server.Data.Factories
{
    public interface ISummaryFactory<T>
    {
        public Summary Build();
    }
}
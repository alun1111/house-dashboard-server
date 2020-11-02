using System.Collections.Generic;
using house_dashboard_server.Data.Models;

namespace house_dashboard_server.Data.Factories
{
    public interface ISnapshotRangeFactory
    {
        public Dictionary<string, HashSet<SnapshotItem>> Build();
    }
}
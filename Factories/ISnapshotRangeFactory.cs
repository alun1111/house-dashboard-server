using System.Collections.Generic;
using house_dashboard_server.Models;

namespace house_dashboard_server.Factories
{
    public interface ISnapshotRangeFactory
    {
        public Dictionary<string, HashSet<SnapshotItem>> Build();
    }
}
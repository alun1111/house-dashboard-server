using System.Collections.Generic;
using HouseDashboardServer.Models;

namespace HouseDashboardServer.Factories
{
    public interface ISnapshotRangeFactory
    {
        public IEnumerable<Snapshot> Build();
    }
}
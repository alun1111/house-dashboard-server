using System;
using System.Collections.Generic;
using HouseDashboardServer.Models;

namespace HouseDashboardServer.Factories
{
    public interface ISnapshotRangeFactory
    {
        public Dictionary<string, HashSet<SnapshotItem>> Build();
    }
}
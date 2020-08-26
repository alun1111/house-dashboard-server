using System;

namespace HouseDashboardServer.Models
{
    public class Snapshot
    {
        public DateTime Moment { get; set; }
        public SnapshotItem Rainfall { get; set; }
        public SnapshotItem RiverLevel { get; set; }
    }

    public class SnapshotItem
    {
        public string Description { get; set; }
        public decimal Value { get; set; }
    }
}
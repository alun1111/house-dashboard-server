using System.Collections.Generic;
using house_dashboard_server.Data.Models;

namespace house_dashboard_server.Formatters
{
    public interface ISnapshotFlattener
    {
        List<string[]> Flatten(Dictionary<string, List<SnapshotItem>> input);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using house_dashboard_server.Data.Models;

namespace house_dashboard_server.Formatters
{
    public class SnapshotFlattener
    {
        public List<object[]> Flatten(Dictionary<string, List<SnapshotItem>> input)
        {
            var snapshot = input.First();
            var output = new List<object[]>();
            var row = new object[snapshot.Value.Count];
            
            for(var x =0; x < snapshot.Value.Count; x++)
            {
                row[x] = snapshot.Value[x];
            }

            output.Add(row);

            return output;
        }
    }
}
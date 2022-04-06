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
            var output = new List<object[]>();
            var outputWithHeader = new List<object[]>();
            var header = new HashSet<string>() { "DateTime" };
            
            foreach (var snapshot in input)
            {
                var numSnapshotValues = snapshot.Value.Count;
                 
                // row array +1 as must include snapshot key (DateTime)
                var row = new object[numSnapshotValues + 1];
                
                // first column always datetime
                row[0] = snapshot.Key;
                
                for(var x = 0; x < numSnapshotValues; x++)
                {
                    header.Add(snapshot.Value[x].Description);
                    row[x+1] = snapshot.Value[x].Value;
                }

                output.Add(row);
            }
            
            outputWithHeader.Add(header.ToArray());
            outputWithHeader.AddRange(output);
            
            return outputWithHeader;
        }
    }
}
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
            var header = new List<string>() { "DateTime" };
            var maxColumns = 0;
            
            foreach (var snapshot in input)
            {
                object[] row;
                
                var numSnapshotValues = snapshot.Value.Count;
                if (numSnapshotValues > maxColumns)
                {
                    maxColumns = numSnapshotValues;
                }
                 
                // max columns as each datetime key may have moer or less values (header columns) but we
                // want nulls in the missing ones
                // also - row array +1 as must include snapshot key (DateTime)
                row = new object[maxColumns + 1];
                
                // first column always datetime
                row[0] = snapshot.Key;
                
                for(var x = 0; x < numSnapshotValues; x++)
                {
                    if (header.Contains(snapshot.Value[x].Description))
                    {
                        var i = header.IndexOf(snapshot.Value[x].Description);
                        row[i] = snapshot.Value[x].Value;
                        continue;
                    }
                    
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
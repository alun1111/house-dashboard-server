using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using house_dashboard_server.Data.Models;

namespace house_dashboard_server.Formatters
{
    public class SnapshotFlattener : ISnapshotFlattener
    {
        public List<string[]> Flatten(Dictionary<string, List<SnapshotItem>> input)
        {
            var output = new List<string[]>();
            var outputWithHeader = new List<string[]>();
            var headerRow = new List<string>() { "DateTime" };
            
            // Shirley more performant way than this... refactor
            foreach (var s in input)
            {
                foreach (var h in s.Value)
                {
                    if (!headerRow.Contains(h.Description))
                    {
                        headerRow.Add(h.Description);
                    }
                }
            }
            
            var headerColumns = headerRow.Count;
            
            foreach (var snapshot in input)
            {
                var row = new string[headerColumns];
                
                // first column always datetime
                row[0] = snapshot.Key;
                
                for(var x = 0; x < snapshot.Value.Count; x++)
                {
                    var columnName = snapshot.Value[x].Description;
                    
                    var columnIndex = headerRow.IndexOf(columnName);
                    row[columnIndex] = snapshot.Value[x].Value.ToString(CultureInfo.InvariantCulture);
                }

                output.Add(row);
            }
            
            outputWithHeader.Add(headerRow.ToArray());
            outputWithHeader.AddRange(output);
            
            return outputWithHeader;
        }
    }
}
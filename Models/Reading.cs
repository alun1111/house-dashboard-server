using System.Collections.Generic;
using HouseDashboardServer.Data;

namespace HouseDashboardServer.Models
{
    public class Reading<T> 
    {
        public Reading(string name, IMeasurement<T> current, List<IMeasurement<T>> recent)
        {
            Name = name;
            Current = current;
            Recent = recent;
        }

        public string Name { get; }
        public IMeasurement<T> Current { get; }
        public List<IMeasurement<T>> Recent { get; }
    }
}

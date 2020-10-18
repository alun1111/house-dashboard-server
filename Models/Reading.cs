using System.Collections.Generic;
using house_dashboard_server.Data.Interfaces;

namespace house_dashboard_server.Models
{
    /// <summary>
    /// A reading is a collection of measurements from a particular source (i.e. station)
    /// </summary>
    /// <typeparam name="T"></typeparam>
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

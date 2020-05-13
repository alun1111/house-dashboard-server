using System.Collections.Generic;
using HouseDashboardServer.Data;

namespace HouseDashboardServer.Models
{
    public class NumberReading<T>
    {
        public NumberReading(string name, IDynamoDbItem<T> current, List<IDynamoDbItem<T>> recent)
        {
            Name = name;
            Current = current;
            Recent = recent;
        }

        public string Name { get; }
        public IDynamoDbItem<T> Current { get; }
        public List<IDynamoDbItem<T>> Recent { get; }
    }
}

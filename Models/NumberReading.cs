using house_dashboard_server.Data;
using System.Collections.Generic;

namespace house_dashboard_server.Models
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

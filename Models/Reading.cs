using house_dashboard_server.Data;
using System.Collections.Generic;

namespace house_dashboard_server.Models
{
    public class Reading<T>
    {
        public Reading(string name, IDynamoDbItem<T> current, List<IDynamoDbItem<T>> recent)
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

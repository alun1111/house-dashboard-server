using house_dashboard_server.Data;
using System.Collections.Generic;
using System.Text;

namespace house_dashboard_server.Models
{
    public class Measurement
    {
        public Measurement(string name, IDynamoDbItem current, List<IDynamoDbItem> recent)
        {
            Name = name;
            Current = current;
            Recent = recent;
        }

        public string Name { get; }
        public IDynamoDbItem Current { get; }
        public List<IDynamoDbItem> Recent { get; }
    }
}

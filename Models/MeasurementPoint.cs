using System;
using System.Collections.Generic;

namespace house_dashboard_server.Models
{
    public class ReadingSet<T>
    {
        public IList<Reading<T>> Readings { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace house_dashboard_server.Models
{
    public class ReadingSet<T>
    {
        public IList<NumberReading<T>> Readings { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace house_dashboard_server.Models
{
    public class ReadingSet
    {
        public IList<Reading<decimal>> Readings { get; set; }
    }
}

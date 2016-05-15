using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class IPMonitorConfig
    {
        public string Timeout { get; set; }

        public string MaxLostCount { get; set; }

        public string MaxRecoverCount { get; set; }

        public string InvalidCount { get; set; }
    }
}

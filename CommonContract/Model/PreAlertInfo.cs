using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonContract.Model
{
    public class PreAlertInfo
    {
        public long SID { get; set; }

        public string IP { get; set; }

        public DateTime FirstLostTime { get; set; }

        public DateTime SecondLostTime { get; set; }

        public DateTime RecoveryTime { get; set; }
    }
}

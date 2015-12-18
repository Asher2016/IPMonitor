using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonContract.Model
{
    public class BrefSendMessageInfo
    {
        public long SID { get; set; }

        public string IP { get; set; }

        public string Telephone { get; set; }

        public DateTime FirstLostTime { get; set; }

        public DateTime SecondLostTime { get; set; }
    }
}

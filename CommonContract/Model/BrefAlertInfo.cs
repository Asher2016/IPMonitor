using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CommonContract.Model
{
    [DataContract]
    [Serializable]
    public class BrefAlertInfo
    {
        [DataMember]
        public long SID { get; set; }

        [DataMember]
        public string IP { get; set; }

        [DataMember]
        public string Region { get; set; }

        [DataMember]
        public string Model { get; set; }

        [DataMember]
        public DateTime FirstLostTime { get; set; }

        [DataMember]
        public DateTime SecondLostTime { get; set; }

        [DataMember]
        public DateTime RecoveryTime { get; set; }

        [DataMember]
        public bool IsSend { get; set; }
    }
}

using CommonConst;
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
    public class IPRegionPair
    {
        [DataMember]
        public string IP { get; set; }

        [DataMember]
        public string Region { get; set; }

        [DataMember]
        public string Status { get; set; }
    }
}

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
    public class IPMonitorListModel
    {
        [DataMember]
        public List<BrefIPRegionInfo> IPRegionList { get; set; }

        [DataMember]
        public long Count { get; set; }
    }
}

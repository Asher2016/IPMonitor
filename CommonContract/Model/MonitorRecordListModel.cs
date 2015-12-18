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
    public class MonitorRecordListModel
    {
        [DataMember]
        public List<BrefIPInfo> BrefIPInfoList { get; set; }

        [DataMember]
        public long Count { get; set; }
    }
}

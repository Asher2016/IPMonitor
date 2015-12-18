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
    public class LogListContract
    {
        [DataMember]
        public List<LogContract> LogInfoList { get; set; }

        [DataMember]
        public long Count { get; set; }

    }
}

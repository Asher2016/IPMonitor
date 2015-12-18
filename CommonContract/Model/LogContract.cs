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
    public class LogContract
    {
        [DataMember]
        public string IpAddress { get; set; }

        [DataMember]
        public string LocalTime { get; set; }

        [DataMember]
        public string RemoteTime { get; set; }

        [DataMember]
        public string LogLevel { get; set; }

        [DataMember]
        public string LogMessage { get; set; }
    }
}

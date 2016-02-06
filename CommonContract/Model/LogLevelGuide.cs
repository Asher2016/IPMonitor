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
    public class LogLevelGuide
    {
        [DataMember]
        public string LogLevel { get; set; }

        [DataMember]
        public short Number { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}

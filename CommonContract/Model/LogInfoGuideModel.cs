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
    public class LogInfoGuideModel
    {
        [DataMember]
        public string KeyWords { get; set; }

        [DataMember]
        public string LogLevel { get; set; }

        [DataMember]
        public string LogInformation { get; set; }

        [DataMember]
        public string Solution { get; set; }
    }
}

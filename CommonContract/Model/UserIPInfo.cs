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
    public class UserIPInfo
    {
        [DataMember]
        public long SID { get; set; }

        [DataMember]
        public string IPAddress { get; set; }

        [DataMember]
        public string UserName { get; set; }
    }
}

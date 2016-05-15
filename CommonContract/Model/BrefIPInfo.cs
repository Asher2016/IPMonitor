using CommonConst;
using System;
using System.Runtime.Serialization;

namespace CommonContract.Model
{
    [DataContract]
    [Serializable]
    public class BrefIPInfo
    {
        [DataMember]
        public string IP { get; set; }

        [DataMember]
        public string Region { get; set; }

        [DataMember]
        public string Model { get; set; }

        [DataMember]
        public DateTime LostTime { get; set; }

        [DataMember]
        public DateTime RecoveryTime { get; set; }
    }
}

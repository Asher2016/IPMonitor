using System;
using System.Runtime.Serialization;

namespace CommonContract.Model
{
    [DataContract]
    [Serializable]
    public class IPStatusView
    {
        [DataMember]
        public string Location { get; set; }

        [DataMember]
        public string Status { get; set; }
    }
}

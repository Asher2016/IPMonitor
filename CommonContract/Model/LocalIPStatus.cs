using System;
using System.Runtime.Serialization;

namespace CommonConst
{
    [DataContract]
    [Serializable]
    public enum LocalIPStatus
    {
        [EnumMember]
        Unknow = 0,

        [EnumMember]
        Unimpeded = 1,

        [EnumMember]
        Impeded = 2,

        [EnumMember]
        Invalid = 3
    }
}

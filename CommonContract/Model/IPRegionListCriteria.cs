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
    public class IPRegionListCriteria
    {
        [DataMember]
        public string SearchColumn { get; set; }

        [DataMember]
        public string SearchText { get; set; }

        [DataMember]
        public string Region { get; set; }

        [DataMember]
        public int PageSize { get; set; }

        [DataMember]
        public int PageIndex { get; set; }
    }
}

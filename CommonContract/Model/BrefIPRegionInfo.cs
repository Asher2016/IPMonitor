using CommonConst;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CommonContract.Model
{
    [DataContract]
    [Serializable]
    public class BrefIPRegionInfo
    {
        [DataMember]
        public long SID { get; set; }

        [DataMember]
        [Required(ErrorMessage = "IP地址不能为空")]
        public string IP { get; set; }

        [DataMember]
        public string Region { get; set; }

        [DataMember]
        public string RegionDisplayName { get; set; }

        [DataMember]
        [Required(ErrorMessage = "型号不能为空")]
        public string Model { get; set; }

        [DataMember]
        [Required(ErrorMessage = "电话不能为空")]
        public string Telephone { get; set; }

        [DataMember]
        public LocalIPStatus Status { get; set; }
    }
}

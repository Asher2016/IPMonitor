using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace DataAccess.Model
{
    [DataContract]
    [Serializable]
    public class DateFormatInfo
    {
        [DataMember]
        public int LocalDateLength { get; set; }

        [DataMember]
        public List<string> LocalDateFormat { get; set; }

        [DataMember]
        public int RemoteDateLength { get; set; }

        [DataMember]
        public List<string> RemoteDateFormat { get; set; }

        [DataMember]
        public char[] TrimChar { get; set; }
    }
}

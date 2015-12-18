using PlatForm.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataAccess.Model
{
    public class CopyLogConfig
    {
        public string JobName { get; set; }

        public string JobGroup { get; set; }

        public string Corn { get; set; }

        public string LogTableStruct { get; set; }

        public SerializableDictionary<string, DateFormatInfo> DateFormatInfoDic { get; set; }

        [XmlIgnore]
        public string DateFormatInfoDicString
        {
            get
            {
                return XmlConvertor.ObjectToXml(DateFormatInfoDic);
            }

            set
            {
                DateFormatInfoDic = XmlConvertor.XmlToObject<SerializableDictionary<string, DateFormatInfo>>(value);
            }
        }

        public CutomerDataBase CustomDB { get; set; }

        [XmlIgnore]
        public string CustomDBString
        {
            get
            {
                return XmlConvertor.ObjectToXml(CustomDB);
            }

            set
            {
                CustomDB = XmlConvertor.XmlToObject<CutomerDataBase>(value);
            }
        }

        public string StartupPath { get; set; }

        public string LogPath { get; set; }

        public string TableStruct { get; set; }
    }
}

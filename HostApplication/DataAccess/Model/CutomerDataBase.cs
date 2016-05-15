using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Model
{
    [DataContract]
    [Serializable]
    public class CutomerDataBase
    {
        [DataMember]
        public string ServerName { get; set; }

        [DataMember]
        public string Database { get; set; }

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string Password { get; set; }
    }
}

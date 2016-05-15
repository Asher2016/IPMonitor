using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonConst
{
    public class RedisConst
    {
        //public const string IPMonitorIPList = "IPMonitorIPList";

        //public const string IPMonitorRecordKey = "IPMonitorRecord-{0}";

        //public const string IPMonitorHistoryKey = "IPMonitorHistory-{0}";

        //public const string IPMonitorAlertKey = "IPMonitorAlert-{0}";

        //public const string IPMonitorRecordKeyLock = "IPMonitorRecord-{0}-Lock";

        //public const string IPMonitorAlertKeyLock = "IPMonitorAlert-{0}-Lock";

        //public const string IPMonitorHistoryKeyLock = "IPMonitorHistory-{0}-Lock";

        public const string IPMonitorIPList = "Monitor-IP-{0}";

        public const string IPMonitorIPLock = "Monitor-IP-{0}-Lock";

        public const string UserLoginPasswdMD5 = "User-Login-MD5-key";

        public const string CurrentMonitorIPList = "CurrentMonitorIPList";
        public const string CurrentMonitorIPLisLock = "CurrentMonitorIPLisLock";
    }
}

using CommonConst;
using CommonContract.Model;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WebApplication.Util
{
    public static class RedisHelper
    {
        private static string RedisIP = ConfigurationManager.ConnectionStrings["ReidsURL"].ToString();

        public const string CurrentMonitorIPList = "CurrentMonitorIPList";
        public const string CurrentMonitorIPLisLock = "CurrentMonitorIPLisLock";
        public const string IPMonitorIPList = "Monitor-IP-{0}";

        public static List<IPRegionPair> GetIPList()
        {
            List<IPRegionPair> result = null;
            using (RedisClient redisClient = new RedisClient(RedisIP))
            {
                using (RedisLock redisLock = new RedisLock(redisClient, CurrentMonitorIPLisLock))
                {
                    if (redisClient.Exists(CurrentMonitorIPList) >= 1)
                    {
                        result = redisClient.Get<List<IPRegionPair>>(CurrentMonitorIPList);
                    }
                }
            }

            return result;
        }

        public static LocalIPStatus GetIPStatus(string ip)
        {
            LocalIPStatus result = LocalIPStatus.Unknow;

            using (RedisClient redisClient = new RedisClient(RedisIP, 6379))
            {
                if (redisClient.Exists(string.Format(IPMonitorIPList, ip)) >= 1)
                {
                    result = redisClient.Get<LocalIPStatus>(string.Format(IPMonitorIPList, ip));
                }
            }

            return result;
        }
    }
}
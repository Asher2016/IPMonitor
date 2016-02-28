using CommonConst;
using CommonContract.Model;
using ServiceStack.Redis;
using System.Collections.Generic;
using System.Configuration;

namespace PlatForm.Util
{
    public class RedisHelper
    {
        private static string RedisIP = ConfigurationManager.ConnectionStrings["ReidsURL"].ToString();

        public static void DeleteCache(string ip)
        {
            using (RedisClient redisClient = new RedisClient(RedisIP))
            {
                using (RedisLock redisLock = new RedisLock(redisClient, RedisConst.CurrentMonitorIPLisLock))
                {
                    redisClient.Remove(string.Format(RedisConst.IPMonitorIPList, ip));
                }
            }
        }

        public static List<IPRegionPair> GetIPList()
        {
            List<IPRegionPair> result = null;
            using (RedisClient redisClient = new RedisClient(RedisIP))
            {
                using (RedisLock redisLock = new RedisLock(redisClient, RedisConst.CurrentMonitorIPLisLock))
                {
                    if (redisClient.Exists(RedisConst.CurrentMonitorIPList) >= 1)
                    {
                        result = redisClient.Get<List<IPRegionPair>>(RedisConst.CurrentMonitorIPList);
                    }
                }
            }

            return result;
        }

        public static void LoadIPList(List<IPRegionPair> ipList)
        {
            using (RedisClient redisClient = new RedisClient(RedisIP))
            {
                using (RedisLock redisLock = new RedisLock(redisClient, RedisConst.CurrentMonitorIPLisLock))
                {
                    if (redisClient.Exists(RedisConst.CurrentMonitorIPList) < 1)
                    {
                        redisClient.Add<List<IPRegionPair>>(RedisConst.CurrentMonitorIPList, ipList);
                    }
                    else
                    {
                        redisClient.Set<List<IPRegionPair>>(RedisConst.CurrentMonitorIPList, ipList);
                    }
                }
            }
        }

        public static void LoadIP(string ip)
        {
            using (RedisClient redisClient = new RedisClient(RedisIP))
            {
                using (RedisLock redisLock = new RedisLock(redisClient, string.Format(RedisConst.IPMonitorIPLock, ip)))
                {
                    if (redisClient.Exists(string.Format(RedisConst.IPMonitorIPList, ip)) < 1)
                    {
                        redisClient.Add(string.Format(RedisConst.IPMonitorIPList, ip), LocalIPStatus.Unimpeded);
                    }
                    else
                    {
                        redisClient.Remove(string.Format(RedisConst.IPMonitorIPList, ip));
                        redisClient.Add(string.Format(RedisConst.IPMonitorIPList, ip), LocalIPStatus.Unimpeded);
                    }
                }
            }
        }

        public static void UpdateIP(string ip, LocalIPStatus ipStatus)
        {
            using (RedisClient redisClient = new RedisClient(RedisIP, 6379))
            {
                //using (RedisLock redisLock = new RedisLock(redisClient, string.Format(RedisConst.IPMonitorIPLock, ip)))
                //{
                    if (redisClient.Exists(string.Format(RedisConst.IPMonitorIPList, ip)) < 1)
                    {
                        redisClient.Add(string.Format(RedisConst.IPMonitorIPList, ip), ipStatus);
                    }
                    else
                    {
                        redisClient.Set(string.Format(RedisConst.IPMonitorIPList, ip), ipStatus);
                    }
                //}
            }
        }

        public static LocalIPStatus GetIPStatus(string ip)
        {
            LocalIPStatus result = LocalIPStatus.Unknow;

            using (RedisClient redisClient = new RedisClient(RedisIP, 6379))
            {
                if (redisClient.Exists(string.Format(RedisConst.IPMonitorIPList, ip)) >= 1)
                {
                    result = redisClient.Get<LocalIPStatus>(string.Format(RedisConst.IPMonitorIPList, ip));
                }
            }

            return result;
        }
    }
}

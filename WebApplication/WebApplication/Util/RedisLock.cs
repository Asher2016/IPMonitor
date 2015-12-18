using ServiceStack.Redis;
using ServiceStack.Redis.Support.Locking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Util
{
    public class RedisLock : IDisposable
    {
        private DisposableDistributedLock distributeLock;

        public RedisLock(RedisClient client, string key, int acquisitionTimeoutSeconds = 10, int timeoutSeconds = 1)
        {
            distributeLock = new DisposableDistributedLock(client, key, acquisitionTimeoutSeconds, timeoutSeconds);
        }

        public void Dispose()
        {
            distributeLock.Dispose();
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace WebApplication.Util
{
    public class WcfClient<T>
    {
        #region Static Fields

        private static Dictionary<string, ChannelFactory<T>> cachedFactories = new Dictionary<string, ChannelFactory<T>>();
        private static object locker = new object();

        #endregion

        #region Wcf Client

        public T GetService(string name)
        {
            ChannelFactory<T> channelFactory = null;
            if (cachedFactories.ContainsKey(name))
            {
                channelFactory = cachedFactories[name];
            }
            else
            {
                channelFactory = new ChannelFactory<T>(name);
                lock (locker)
                {
                    if (!cachedFactories.ContainsKey(name))
                    {
                        cachedFactories[name] = channelFactory;
                    }
                }
            }

            T proxy = channelFactory.CreateChannel();
            return proxy;
        }

        #endregion
    }
}
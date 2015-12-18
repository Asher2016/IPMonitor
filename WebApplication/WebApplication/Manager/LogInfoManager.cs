using CommonContract.Model;
using CommonContract.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using WebApplication.Util;

namespace WebApplication.Manager
{
    public class LogInfoManager : ILogService
    {
        WcfClient<ILogService> client = new WcfClient<ILogService>();
        ILogService service = null;

        public LogInfoManager()
        {
            try
            {
                service = client.GetService("ILogService");
            }
            catch (FaultException exception)
            {
                LogHelper.Instance.Error(LogHelper.WebSite, exception.Message);
            }
        }

        public LogListContract SearchList(LogCriteria criteria)
        {
            LogListContract result = new LogListContract();

            try
            {
                result = service.SearchList(criteria);

                if (result.Count == 0)
                {
                    result.Count = 1;
                }
            }
            catch (FaultException exception)
            {
                LogHelper.Instance.Error(LogHelper.WebSite, exception.Message);
                throw new Exception("Internal Server Error.", exception.InnerException);
            }

            return result;
        }
    }
}
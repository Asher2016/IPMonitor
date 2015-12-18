using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication.Manager;
using CommonContract.Model;
using WebApplication.Util;

namespace WebApplication.Facad
{
    public class LogInformationFacad
    {
        LogInfoManager manager = new LogInfoManager();
        public LogListContract SearchLogInfo(LogCriteria criteria)
        {
            LogListContract list = null;

            try
            {
                list = manager.SearchList(criteria);
            }
            catch(Exception exception)
            {
                LogHelper.Instance.Error(LogHelper.WebSite, exception.Message);
            }

            return list;
        }
    }
}
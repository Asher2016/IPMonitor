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

        public List<LogLevelGuide> GetLogLevelGuideList()
        {
            List<LogLevelGuide> result = null;

            result = manager.GetLogLevelGuideList();

            return result;
        }

        public LogInfoGuideList SearchLogInfoGuideList(LogInfoGuideCriteria criteria)
        {
            LogInfoGuideList result = null;

            result = manager.SearchLogInfoGuideList(criteria);

            return result;
        }
    }
}
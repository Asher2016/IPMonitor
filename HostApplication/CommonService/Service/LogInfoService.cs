using CommonContract.Model;
using CommonContract.Service;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonService.Service
{
    public class LogInfoService : ILogService
    {
        public LogListContract SearchList(LogCriteria criteria)
        {
            LogListContract result = null;

            using (LogInfoDAO dao = new LogInfoDAO())
            {
                result = dao.SearchList(criteria);
            }

            return result;
        }

        public LogInfoGuideList SearchLogInfoGuideList(LogInfoGuideCriteria criteria)
        {
            LogInfoGuideList result = null;

            using (LogInfoGuide dao = new LogInfoGuide())
            {
                result = dao.SearchLogInfoGuideList(criteria);
            }

            return result;
        }

        public List<LogLevelGuide> GetLogLevelGuideList()
        {
            List<LogLevelGuide> result = null;

            using (LogInfoGuide dao = new LogInfoGuide())
            {
                result = dao.GetLogLevelGuideList();
            }

            return result;
        }
    }
}

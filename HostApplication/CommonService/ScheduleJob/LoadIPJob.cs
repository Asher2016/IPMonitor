using CommonContract.Model;
using DataAccess.DAO;
using PlatForm.Util;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonService.ScheduleJob
{
    public class LoadIPJob : IJob
    {
        List<IPRegionPair> result = null;

        public void Execute(IJobExecutionContext context)
        {
            using (IPMonitorDAO dao = new IPMonitorDAO())
            {
                result = dao.GetIpListForMonitor();
            }

            RedisHelper.LoadIPList(result);
        }
    }
}

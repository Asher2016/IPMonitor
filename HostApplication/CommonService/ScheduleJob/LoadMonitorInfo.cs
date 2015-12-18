using CommonContract.Model;
using DataAccess.DAO;
using PlatForm.Util;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CommonService.ScheduleJob
{
    public class LoadMonitorInfo : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            List<IPRegionPair> result = null;

            try
            {
                List<BrefIPInfo> alertList = IPMonitorHelper.GetAlertInfo();
                List<BrefIPInfo> RecordList = IPMonitorHelper.GetRecord();

                using (IPMonitorDAO dao = new IPMonitorDAO())
                {
                    dao.LoadMonitorRecord(RecordList);
                }

                using (AlertDAO dao = new AlertDAO())
                {
                    dao.LoadAlertRecord(alertList);
                }

                IPMonitorHelper.StopMonitorThread();

                using (IPMonitorDAO dao = new IPMonitorDAO())
                {
                    result = dao.GetIpListForMonitor();
                }

                RedisHelper.LoadIPList(result);

                IPMonitorHelper.PushIPStack(result.Select(x => x.IP).Distinct().ToList());

                IPMonitorHelper.StartMonitorThread();
            }
            catch (Exception e)
            {
                LogHelper.Instance.Error(LogHelper.CommonService, "Execute GetIpListForMonitor error " + e.Message);
                throw new FaultException("Internal Server Error");
            }
        }
    }
}

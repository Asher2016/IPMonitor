using CommonContract.Model;
using DataAccess.DAO;
using PlatForm.Util;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
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
                IPMonitorHelper.StopMonitorThread();
                 

                Thread.Sleep(15000);
                List<BrefAlertInfo> alertList = IPMonitorHelper.GetAlertInfo();
                List<BrefIPInfo> recordList = IPMonitorHelper.GetRecord();

                alertList = alertList.Where(alert => alert.FirstLostTime.AddSeconds(3) > alert.SecondLostTime).ToList();
                
                using (IPMonitorDAO dao = new IPMonitorDAO())
                {
                    dao.LoadMonitorRecord(recordList);
                }

                using (AlertDAO dao = new AlertDAO())
                {
                    dao.LoadAlertRecord(alertList);
                }

                using (IPMonitorDAO dao = new IPMonitorDAO())
                {
                    result = dao.GetIpListForMonitor();
                }

                RedisHelper.LoadIPList(result);

                IPMonitorHelper.PushIPStack(result.Select(x => x.IP).Distinct().ToList());

                List<BrefAlertInfo> preAlertList = new List<BrefAlertInfo>();

                using (AlertDAO dao = new AlertDAO())
                {
                    preAlertList = dao.GetPreNotRecoveryAlert();
                }

                IPMonitorHelper.SetPreAlertInfo(preAlertList);

                IPMonitorHelper.CleanData();
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

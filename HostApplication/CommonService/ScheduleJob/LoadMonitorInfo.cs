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

                List<BrefAlertInfo> alertList = IPMonitorHelper.GetAlertInfo();
                List<BrefIPInfo> recordList = IPMonitorHelper.GetRecord();

                alertList = alertList.Where(alert =>
                    (
                        alert.SecondLostTime > alert.FirstLostTime &&
                        (alert.RecoveryTime == null || alert.RecoveryTime == DateTime.MinValue || alert.RecoveryTime > alert.SecondLostTime) &&
                        alert.SecondLostTime.Day == alert.FirstLostTime.Day &&
                        alert.SecondLostTime.Hour == alert.FirstLostTime.Hour &&
                        alert.SecondLostTime.Second - alert.FirstLostTime.Second >= 1 &&
                        alert.SecondLostTime.Hour - alert.FirstLostTime.Hour < 1
                    )).ToList();

                alertList = alertList.Where(alert => alert.SID == 0 || alert.SID != 0 && (alert.RecoveryTime != DateTime.MinValue)).ToList();

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

                //List<IPRegionPair> testIP = new List<IPRegionPair>();
                //testIP.Add(new IPRegionPair() { IP = "www.baidu.com", Region = "PianGuan" });
                //testIP.Add(new IPRegionPair() { IP = "127.0.0.1", Region = "PianGuan" });
                //testIP.Add(new IPRegionPair() { IP = "11.1.1.1", Region = "PianGuan" });
                //testIP.Add(new IPRegionPair() { IP = "www.youku.com", Region = "PianGuan" });
                //testIP.Add(new IPRegionPair() { IP = "www.163.com", Region = "PianGuan" });

                //RedisHelper.LoadIPList(testIP);

                //IPMonitorHelper.PushIPStack(testIP.Select(x => x.IP).Distinct().ToList());

                RedisHelper.LoadIPList(result);

                IPMonitorHelper.PushIPStack(result.Select(x => x.IP).Distinct().ToList());

                List<BrefAlertInfo> preAlertList = new List<BrefAlertInfo>();

                using (AlertDAO dao = new AlertDAO())
                {
                    preAlertList = dao.GetPreNotRecoveryAlert();
                }

                IPMonitorHelper.CleanData();

                IPMonitorHelper.SetPreAlertInfo(preAlertList);
                
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

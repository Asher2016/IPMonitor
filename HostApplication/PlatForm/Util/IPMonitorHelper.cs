using CommonConst;
using CommonContract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PlatForm.Util
{
    public class IPMonitorHelper
    {
        private static CancellationTokenSource currentCTS = new CancellationTokenSource();
        private static Stack<string> IPStack = new Stack<string>();
        //private static Dictionary<string, BrefIPInfo> ErrorLevelRecord = new Dictionary<string, BrefIPInfo>();
        //private static Dictionary<string, BrefIPInfo> ErrorLevelHistory = new Dictionary<string, BrefIPInfo>();
        //private static Dictionary<string, BrefIPInfo> ErrorLevelAlert = new Dictionary<string, BrefIPInfo>();
        //private static List<string> tempList = new List<string>();

        private static object lostRecordLock = new Object();
        private static List<BrefIPInfo> lostRecord = new List<BrefIPInfo>();

        private static object recordRecoveryListLock = new Object();
        private static List<string> recordRecoveryList = new List<string>();

        private static object recoveryRecordLock = new Object();
        private static List<BrefIPInfo> recoveryRecord = new List<BrefIPInfo>();

        private static object invalidRecordLock = new Object();
        private static List<string> invalidRecord = new List<string>();

        private static object alertListLock = new Object();
        private static List<BrefAlertInfo> alertList = new List<BrefAlertInfo>();

        private static object alertRecoveryListLock = new Object();
        private static List<string> alertRecoveryList = new List<string>();

        public static void PushIPStack(List<string> ipList)
        {
            foreach (string ipItem in ipList)
            {
                IPStack.Push(ipItem);
            }
        }

        public static void StopMonitorThread()
        {
            currentCTS.Cancel();
            currentCTS.Dispose();

            lock (invalidRecordLock)
            {
                invalidRecord.Clear();
            }

            lock (recoveryRecordLock)
            {
                recordRecoveryList.Clear();
            }
        }   

        public static List<BrefAlertInfo> GetAlertInfo()
        {
            List<BrefAlertInfo> list = new List<BrefAlertInfo>();

            lock (alertListLock)
            {
                if (alertList.Count > 0)
                {
                    foreach (BrefAlertInfo info in alertList)
                    {
                        list.Add(info);
                    }
                }

                alertList.Clear();
               
            }

            lock (alertRecoveryListLock)
            {
                alertRecoveryList.Clear();
            }

            return list;
        }

        public static void SetPreAlertInfo(List<BrefAlertInfo> list)
        {
            lock (alertListLock)
            {
                alertList.Clear();
                alertList.AddRange(list);

            }

            lock (alertRecoveryListLock)
            {
                alertRecoveryList.Clear();
                alertRecoveryList.AddRange(list.Select(x => x.IP));
            }
        }

        public static void CleanData()
        {
            lock (alertListLock)
            {
                alertList.Clear();
            }
            
            lock (alertRecoveryListLock)
            {
                alertRecoveryList.Clear();
            }
            
            lock (lostRecordLock)
            {
                lostRecord.Clear();
            }

            lock (recoveryRecordLock)
            {
                recoveryRecord.Clear();
            }
        }

        public static List<BrefIPInfo> GetRecord()
        {
            List<BrefIPInfo> list = new List<BrefIPInfo>();

            lock (lostRecordLock)
            {
                if (lostRecord.Count > 0)
                {
                    foreach (BrefIPInfo info in lostRecord)
                    {
                        list.Add(info);
                    }
                }

                lostRecord.Clear();
            }

            lock (recoveryRecordLock)
            {
                if (recoveryRecord.Count > 0)
                {
                    foreach (BrefIPInfo info in recoveryRecord)
                    {
                        list.Add(info);
                    }
                }

                recoveryRecord.Clear();
            }

            return list;
        }

        public static void StartMonitorThread()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            int count = IPStack.Count;
            TaskFactory taskFactory = new TaskFactory();
            Task[] tasks = new Task[IPStack.Count];

            for (int i = 0; i < count; i++)
            {
                tasks[i] = taskFactory.StartNew(() => MonitorIP(cts.Token), cts.Token);
            }

            taskFactory.ContinueWhenAll(tasks, TasksEnded);
            currentCTS = cts;
        }

        private static void TasksEnded(Task[] tasks)
        { }

        private static void MonitorIP(CancellationToken ct)
        {
            string ip = IPStack.Pop();

            RedisHelper.LoadIP(ip);

            Ping p = new Ping();
            PingOptions options = new PingOptions();

            while (!ct.IsCancellationRequested)
            {
                try
                {
                    PingReply reply = p.Send(ip, 2000);

                    if (reply.Status == IPStatus.Success)
                    {
                        ReplySuccessNew(ip);
                    }
                    else
                    {
                        ReplyFailNew(ip);
                    }
                }
                catch
                {
                    ReplyFailNew(ip);
                }
                finally
                {
                    Thread.Sleep(1000);
                }
            }
        }

        #region old
        private static void ReplySuccess(string ip)
        {
            if (recordRecoveryList.Contains(ip))
            {
                DateTime firstLostTime = lostRecord.OrderBy(x => x.LostTime).Where(x => x.IP == ip).FirstOrDefault().LostTime;
                recoveryRecord.Add(new BrefIPInfo() { IP = ip, RecoveryTime = DateTime.Now, LostTime = firstLostTime });
                recordRecoveryList = recordRecoveryList.Where(x => x != ip).ToList();
                lostRecord = lostRecord.Where(x => x.IP != ip && x.LostTime != firstLostTime).ToList();

                if (invalidRecord.Contains(ip))
                {
                    invalidRecord = invalidRecord.Where(x => x != ip).ToList();
                }
            }

            if (alertRecoveryList.Count > 0 && alertRecoveryList.Exists(x => x == ip))
            {
                foreach (BrefAlertInfo item in alertList)
                {
                    if (item.IP.Equals(ip))
                    {
                        item.RecoveryTime = DateTime.Now;
                    }
                }

                alertRecoveryList = alertRecoveryList.Where(x => x != ip).ToList();
            }

            RedisHelper.UpdateIP(ip, LocalIPStatus.Unimpeded);
        }
        #endregion

        private static void ReplySuccessNew(string ip)
        {
            // Checks invalid record.
            if (invalidRecord.Contains(ip))
            {
                lock (invalidRecordLock)
                {
                    invalidRecord.Remove(ip);
                }
            }

            // Checks lost record.
            if (recordRecoveryList.Contains(ip))
            {
                lock (recordRecoveryListLock)
                {
                    recordRecoveryList.Remove(ip);
                }

                DateTime firstLostTime = lostRecord.OrderBy(x => x.LostTime).Where(x => x.IP == ip).FirstOrDefault().LostTime;

                lock (recoveryRecordLock)
                {
                    recoveryRecord.Add(new BrefIPInfo() { IP = ip, RecoveryTime = DateTime.Now, LostTime = firstLostTime });
                }

                lock (lostRecordLock )
                {
                    lostRecord = lostRecord.Where(x => x.IP != ip).ToList();
                }
            }

            // Checks alert record.
            if (alertRecoveryList.Contains(ip))
            {
                lock (alertRecoveryListLock)
                {
                    alertRecoveryList.Remove(ip);
                }
                

                foreach (BrefAlertInfo item in alertList)
                {
                    if (item.IP.Equals(ip))
                    {
                        item.RecoveryTime = DateTime.Now;
                    }
                }
            }

            RedisHelper.UpdateIP(ip, LocalIPStatus.Unimpeded);
        }

        private static void ReplyFailNew(string ip)
        {
            if (!invalidRecord.Contains(ip))
            {
                if (!recordRecoveryList.Contains(ip))
                {
                    lock (recordRecoveryListLock)
                    {
                        recordRecoveryList.Add(ip);
                    }
                }

                if (lostRecord.Where(x => x.IP == ip).Count() > 10)
                {
                    if (!invalidRecord.Contains(ip))
                    {
                        lock (invalidRecordLock)
                        {
                            invalidRecord.Add(ip);
                        }
                        
                        RedisHelper.UpdateIP(ip, LocalIPStatus.Invalid);
                    }
                } else {
                    if (lostRecord.Where(x => x.IP == ip).Count() > 0)
                    {
                        DateTime preIPDateTime = lostRecord.Where(x => x.IP == ip)
                            .OrderByDescending(x => x.LostTime)
                            .Select(x => x.LostTime)
                            .FirstOrDefault();

                        if (preIPDateTime.AddSeconds(15) > DateTime.Now)
                        {
                            if (alertRecoveryList.Contains(ip))
                            {
                                foreach (BrefAlertInfo alert in alertList)
                                {
                                    if (alert.IP == ip)
                                    {
                                        alert.SecondLostTime = DateTime.Now;
                                    }
                                }
                            }
                            else
                            {
                                lock (alertListLock)
                                {
                                    alertList.Add(new BrefAlertInfo() { SID = 0, IP = ip, FirstLostTime = preIPDateTime, SecondLostTime = DateTime.Now });
                                }
                            }

                            if (!alertRecoveryList.Exists(x => x == ip))
                            {
                                lock (alertRecoveryListLock)
                                {
                                    alertRecoveryList.Add(ip);
                                }
                            }
                        }
                    }

                    lock (lostRecordLock)
                    {
                        lostRecord.Add(new BrefIPInfo() { IP = ip, LostTime = DateTime.Now });
                    }
                    
                    RedisHelper.UpdateIP(ip, LocalIPStatus.Impeded);
                }
            }
            else
            {
                RedisHelper.UpdateIP(ip, LocalIPStatus.Invalid);
            }
        }

        #region old
        private static void ReplyFail(string ip)
        {
            if (!invalidRecord.Contains(ip))
            {
                if (!recordRecoveryList.Contains(ip))
                {
                    recordRecoveryList.Add(ip);
                }

                if (lostRecord.Where(x => x.IP == ip).Count() > 10)
                {
                    if (!invalidRecord.Contains(ip))
                    {
                        invalidRecord.Add(ip);
                        RedisHelper.UpdateIP(ip, LocalIPStatus.Invalid);
                    }
                }
                else
                {
                    DateTime preIPDateTime = lostRecord.Where(x => x.IP == ip)
                        .OrderByDescending(x => x.LostTime)
                        .Select(x => x.LostTime)
                        .FirstOrDefault();

                    if (preIPDateTime.AddSeconds(15) > DateTime.Now)
                    {
                        if (alertList.Exists(x => x.IP == ip))
                        {
                            alertList = alertList.Where(x => x.IP != ip).ToList();
                            alertList.Add(new BrefAlertInfo() { IP = ip, FirstLostTime = preIPDateTime, SecondLostTime = DateTime.Now });
                        }
                        else
                        {
                            alertList.Add(new BrefAlertInfo() { IP = ip, FirstLostTime = preIPDateTime, SecondLostTime = DateTime.Now });
                        }

                        if (!alertRecoveryList.Exists(x => x == ip))
                        {
                            alertRecoveryList.Add(ip);
                        }
                    }

                    lostRecord.Add(new BrefIPInfo() { IP = ip, LostTime = DateTime.Now });
                    RedisHelper.UpdateIP(ip, LocalIPStatus.Impeded);
                }
            }
            else {
                RedisHelper.UpdateIP(ip, LocalIPStatus.Invalid);
            }
        }

        #endregion
    }
}

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
        private static CancellationTokenSource cts = new CancellationTokenSource();
        private static Stack<string> IPStack = new Stack<string>();
        private static Dictionary<string, BrefIPInfo> ErrorLevelRecord = new Dictionary<string, BrefIPInfo>();
        private static Dictionary<string, BrefIPInfo> ErrorLevelHistory = new Dictionary<string, BrefIPInfo>();
        private static Dictionary<string, BrefIPInfo> ErrorLevelAlert = new Dictionary<string, BrefIPInfo>();
        private static List<string> tempList = new List<string>();

        private static object lockForRecord = new object();
        private static object lockForAlertRecord = new object();

        private static List<BrefIPInfo> lostRecord = new List<BrefIPInfo>();
        private static List<string> recordRecoveryList = new List<string>(); //wait for recovery
        private static List<BrefIPInfo> recoveryRecord = new List<BrefIPInfo>();
        private static List<string> invalidRecord = new List<string>();
        private static List<BrefAlertInfo> alertList = new List<BrefAlertInfo>();
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
            cts.Cancel();
            invalidRecord.Clear();
            recordRecoveryList.Clear();
        }

        public static List<BrefAlertInfo> GetAlertInfo()
        {
            List<BrefAlertInfo> list = new List<BrefAlertInfo>();

            lock (lockForRecord)
            {
                if (alertList.Count > 0)
                {
                    foreach (BrefAlertInfo info in alertList)
                    {
                        list.Add(info);
                    }
                }

                alertList.Clear();
                alertRecoveryList.Clear();
            }

            return list;
        }

        public static void SetPreAlertInfo(List<BrefAlertInfo> list)
        {
            lock (lockForRecord)
            {
                alertList.Clear();
                alertList.AddRange(list);
                alertRecoveryList.Clear();
                alertRecoveryList.AddRange(list.Select(x => x.IP));
            }
        }

        public static void CleanData()
        {
            alertList.Clear();
            alertRecoveryList.Clear();

            lostRecord.Clear();
            recoveryRecord.Clear();
        }

        public static List<BrefIPInfo> GetRecord()
        {
            List<BrefIPInfo> list = new List<BrefIPInfo>();

            lock (lockForRecord)
            {
                if (lostRecord.Count > 0)
                {
                    foreach (BrefIPInfo info in lostRecord)
                    {
                        list.Add(info);
                    }
                }

                lostRecord.Clear();

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
            int count = IPStack.Count;

            TaskFactory taskFactory = new TaskFactory();
            Task[] tasks = new Task[IPStack.Count];

            for (int i = 0; i < count; i++)
            {
                tasks[i] = taskFactory.StartNew(() => MonitorIP(cts.Token));
            }

            taskFactory.ContinueWhenAll(tasks, TasksEnded, CancellationToken.None);
        }

        private static void TasksEnded(Task[] tasks)
        {
            //Does Nothing.
        }

        private static void MonitorIP(CancellationToken ct)
        {
            string ip = IPStack.Pop();

            RedisHelper.LoadIP(ip);

            LogHelper.Instance.Info(LogHelper.CommonService, "TaskStart……" + ip);
            string data = "Test Data!";
            Ping p = new Ping();
            PingOptions options = new PingOptions();

            while (true)
            {
                data = "Test Data!";
                for (int i = 0; i < 20; i++)
                {
                    byte[] buffer = Encoding.ASCII.GetBytes(data);
                    System.Net.NetworkInformation.PingReply reply = p.Send(ip, 2000, buffer, options);
                    string replyData = Encoding.Default.GetString(reply.Buffer);

                    if (replyData.Equals(data))
                    {
                        lock (lockForRecord)
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

                                RedisHelper.UpdateIP(ip, LocalIPStatus.Unimpeded);
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
                        }
                    }
                    else
                    {
                        lock(lockForRecord)
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
                                    DateTime preIPDateTime = lostRecord.Where(x => x.IP == ip).OrderByDescending(x => x.LostTime)
                                        .Select(x => x.LostTime)
                                        .FirstOrDefault();

                                    if (preIPDateTime.AddSeconds(10) > DateTime.Now)
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
                        }
                    }

                    Thread.Sleep(300);
                }
            }
        }
    }
}

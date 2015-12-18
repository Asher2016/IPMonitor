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
        private static object lockForHistory = new object();
        private static object lockForAlert = new object();
        private static object lockForTempList = new object();

        private static List<BrefIPInfo> lostRecord = new List<BrefIPInfo>();
        private static List<string> recordRecoveryList = new List<string>(); //wait for recovery
        private static List<BrefIPInfo> recoveryRecord = new List<BrefIPInfo>();
        private static List<string> invalidRecord = new List<string>();
        private static List<BrefIPInfo> alertList = new List<BrefIPInfo>();
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
            lostRecord.Clear();
        }

        public static List<BrefIPInfo> GetAlertInfo()
        {
            List<BrefIPInfo> list = new List<BrefIPInfo>();

            lock(lockForAlert)
            {
                if (alertList.Count > 0)
                {
                    foreach (BrefIPInfo info in alertList)
                    {
                        list.Add(info);
                    }
                }

                alertList.Clear();
            }

            return list;
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
            }

            lock (lockForHistory)
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
                                recoveryRecord.Add(new BrefIPInfo() { IP = ip, RecoveryTime = DateTime.Now, LostTime = lostRecord.OrderBy(x => x.LostTime).Where(x => x.IP == ip).FirstOrDefault().LostTime });
                                recordRecoveryList.Remove(ip);

                                if (invalidRecord.Contains(ip))
                                {
                                    invalidRecord.Remove(ip);
                                }

                                RedisHelper.UpdateIP(ip, LocalIPStatus.Unimpeded);
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
                                    DateTime preIPDateTime = lostRecord.Where(x => x.IP == ip).OrderBy(x => x.LostTime)
                                        .Select(x => x.LostTime)
                                        .FirstOrDefault();

                                    if (preIPDateTime.AddSeconds(10) > DateTime.Now
                                        && alertList.Where(x => x.IP == ip).Count() == 0)
                                    {

                                        alertList.Add(new BrefIPInfo() { IP = ip, LostTime = preIPDateTime, RecoveryTime = DateTime.Now });
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

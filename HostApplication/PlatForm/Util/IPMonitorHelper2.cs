using CommonConst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PlatForm.Util
{
    public class IPMonitorHelper2
    {
        private static CancellationTokenSource cts = new CancellationTokenSource();
        private static Stack<string> IPStack = new Stack<string>();
        private static Dictionary<string, BrefIPInfo> ErrorLevelRecord = new Dictionary<string, BrefIPInfo>();//第一次丢包 10s 清除一次

        private static Dictionary<string, BrefIPInfo> ErrorLevelHistory = new Dictionary<string, BrefIPInfo>();
        private static Dictionary<string, BrefIPInfo> ErrorLevelAlert = new Dictionary<string, BrefIPInfo>();
        private static List<string> tempList = new List<string>();

        private static object lockForRecord = new object();
        private static object lockForHistory = new object();
        private static object lockForAlert = new object();
        private static object lockForTempList = new object();

        private static List<BrefIPInfo> records = new List<BrefIPInfo>();

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
        }

        public static List<BrefIPInfo> GetAlertInfo()
        {
            List<BrefIPInfo> list = new List<BrefIPInfo>();

            lock (lockForAlert)
            {
                if (ErrorLevelAlert.Count > 0)
                {
                    foreach (string key in ErrorLevelAlert.Keys)
                    {
                        list.Add(ErrorLevelAlert[key]);
                    }
                }

                ErrorLevelAlert.Clear();
            }

            return list;
        }

        public static List<BrefIPInfo> GetHistory()
        {
            List<BrefIPInfo> list = new List<BrefIPInfo>();

            lock (lockForHistory)
            {
                if (ErrorLevelHistory.Count > 0)
                {
                    foreach (string key in ErrorLevelHistory.Keys)
                    {
                        list.Add(ErrorLevelHistory[key]);
                    }
                }

                ErrorLevelHistory.Clear();
            }

            return list;
        }

        public static List<BrefIPInfo> GetRecord()
        {
            List<BrefIPInfo> list = new List<BrefIPInfo>();

            lock (lockForRecord)
            {
                if (ErrorLevelRecord.Count > 0)
                {
                    foreach (string key in ErrorLevelRecord.Keys)
                    {
                        list.Add(ErrorLevelRecord[key]);
                    }
                }

                ErrorLevelRecord.Clear();
            }

            return list;
        }

        public static void StartMonitorThread()
        {
            int count = IPStack.Count;

            TaskFactory taskFactory = new TaskFactory();
            Task[] tasks = new Task[IPStack.Count + 1];

            for (int i = 0; i < count; i++)
            {
                tasks[i] = taskFactory.StartNew(() => MonitorIP(cts.Token));
            }

            tasks[count] = taskFactory.StartNew(() => ErrorLevelAlertAndSaveIntoDB(cts.Token));
            taskFactory.ContinueWhenAll(tasks, TasksEnded, CancellationToken.None);
        }

        private static void TasksEnded(Task[] tasks)
        {
            //Does Nothing.
        }

        private static void ErrorLevelAlertAndSaveIntoDB(CancellationToken ct)
        {
            while (true)
            {
                if (ErrorLevelRecord.Count > 0)
                {
                    foreach (string ip in ErrorLevelRecord.Keys)
                    {
                        if (ErrorLevelRecord[ip].LoseTimeFirst != DateTime.MinValue
                            && ErrorLevelRecord[ip].LoseTimeSecond != DateTime.MinValue
                            && ErrorLevelRecord[ip].LoseTimeFirst.AddSeconds(10) > ErrorLevelRecord[ip].LoseTimeSecond)
                        {
                            if (!ErrorLevelAlert.ContainsKey(ip))
                            {
                                ErrorLevelAlert.Add(ip, ErrorLevelRecord[ip]);

                                lock (lockForTempList)
                                {
                                    tempList.Add(ip);
                                }

                                LogHelper.Instance.Info(LogHelper.CommonService, string.Format("in alert--{0}", ip));
                            }

                        }
                    }

                    if (tempList.Count > 0)
                    {
                        lock (lockForTempList)
                        {
                            foreach (string ip in tempList)
                            {
                                ErrorLevelRecord.Remove(ip);
                                LogHelper.Instance.Info(LogHelper.CommonService, string.Format("clear--temp remove ip {0}", ip));
                            }

                            tempList.Clear();
                            LogHelper.Instance.Info(LogHelper.CommonService, string.Format("clear--temp list"));
                        }
                    }
                }

                Thread.Sleep(10000);
            }
        }

        private static void MonitorIP(CancellationToken ct)
        {
            string ip = IPStack.Pop();

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
                    System.Net.NetworkInformation.PingReply reply = p.Send(ip, 5000, buffer, options);
                    string replyData = Encoding.Default.GetString(reply.Buffer);

                    if (replyData.Equals(data))
                    {
                        if (ErrorLevelHistory.ContainsKey(ip))
                        {
                            lock (lockForHistory)
                            {
                                ErrorLevelHistory[ip].RecoveryTime = DateTime.Now;
                            }
                            LogHelper.Instance.Info(LogHelper.CommonService, string.Format("{2} recovery, SendData:{0}, ReplayData:{1}", data, replyData, ip));
                        }
                    }
                    else
                    {
                        if (!ErrorLevelHistory.ContainsKey(ip))
                        {
                            lock (lockForHistory)
                            {
                                ErrorLevelHistory.Add(ip, new BrefIPInfo() { IP = ip, LoseTimeFirst = DateTime.Now });
                            }
                        }

                        lock (lockForRecord)
                        {
                            if (ErrorLevelRecord.ContainsKey(ip))
                            {
                                if (ErrorLevelRecord[ip].LoseTimeSecond == DateTime.MinValue)
                                {
                                    ErrorLevelRecord[ip].LoseTimeSecond = DateTime.Now;
                                }
                                else
                                {
                                    ErrorLevelRecord[ip].LoseTimeFirst = ErrorLevelRecord[ip].LoseTimeSecond;
                                    ErrorLevelRecord[ip].LoseTimeSecond = DateTime.Now;
                                }
                            }
                            else
                            {
                                ErrorLevelRecord.Add(ip, new BrefIPInfo() { IP = ip, LoseTimeFirst = DateTime.Now });
                            }
                        }

                        LogHelper.Instance.Info(LogHelper.CommonService, string.Format("{2} lost,  SendData:{0}, ReplayData:{1}", data, replyData, ip));
                    }

                    Thread.Sleep(300);
                }
            }
        }
    }
}

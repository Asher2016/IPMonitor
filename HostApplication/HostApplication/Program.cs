using CommonConst;
using CommonContract.Model;
using CommonService.Manager;
using CommonService.Service;
using DataAccess.DAO;
using PlatForm.Util;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Threading;

namespace HostApplication
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            //Thread.Sleep(3000);
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            { 
                new HostedApplicationServer()
            };

            ServiceBase.Run(ServicesToRun);

            //ScheduleJobManager.Instance.StartCopyLogInfo();
            //ScheduleJobManager.Instance.StartMonitorIPJob();
            //ScheduleJobManager.Instance.StartSendMessageJob();

            ////string ss = DateTime.Now.ToString("MMM dd HH:mm:ss");
            ////DateTime s = DateTime.ParseExact("Nov 09 15:49:11", "MMM dd HH:mm:ss", CultureInfo.InvariantCulture);

            ////SerializableDictionary<string, DateFormatInfo> dic = new SerializableDictionary<string, DateFormatInfo>();

            ////DateFormatInfo local7 = new DateFormatInfo()
            ////{
            ////    LocalDateLength = 15,
            ////    LocalDateFormat = new List<string>()
            ////    {
            ////        "MMM dd HH:mm:ss"
            ////    },
            ////    RemoteDateLength = 20,
            ////    RemoteDateFormat = new List<string>()
            ////    {
            ////        "MMM  d HH:mm:ss yyyy",
            ////        "MMM dd HH:mm:ss yyyy",
            ////        "MMM  d yyyy HH:mm:ss",
            ////        "MMM dd yyyy HH:mm:ss"
            ////    },
            ////    TrimChar = new char[] { ' ' }
            ////};

            ////dic.Add("local7.log", local7);

            ////DateFormatInfo local5 = new DateFormatInfo()
            ////{
            ////    LocalDateLength = 15,
            ////    LocalDateFormat = new List<string>()
            ////    {
            ////        "MMM dd HH:mm:ss"
            ////    },
            ////    RemoteDateLength = 17,
            ////    RemoteDateFormat = new List<string>()
            ////    {
            ////        "MMM  d HH:mm:ss",
            ////        "MMM dd HH:mm:ss"
            ////    },
            ////    TrimChar = new char[] { '*', ':', ' ' }
            ////};

            ////dic.Add("local5.log", local5);
            ////CopyLogConfig copyLogConfig = new CopyLogConfig();
            ////copyLogConfig.Corn = "0 0 0 ? * * ";
            ////copyLogConfig.JobName = "CopyJobInfo";
            ////copyLogConfig.JobGroup = 

            ////string xxx = XmlConvertor.ObjectToXml(dic);
            ////Serializer(typeof(SerializableDictionary<string, DateFormatInfo>), dic);

            //string LogFile = "D:\\WorkSpace\\logInfo";
            //string s2 = new TranslateFileFormat().ConvertFormatToCSV(LogFile, dic);

            //LogFileManager manager = new LogFileManager();
            //manager.RunCopy();

            ////new ScheduleJobManager().StartCopyLogInfo();

            ////SerializableDictionary<string, DateFormatInfo> dics =
            ////    XmlConvertor.XmlToObject<SerializableDictionary<string, DateFormatInfo>>(config);

           ////string sss = ConnectionUtil.ConnString;

            ////LogHelper.Instance.Info(LogHelper.commonService,"ss2");
            ////LogHelper.Instance.Debug(LogHelper.commonService,"ss2222222222");
            ////LogHelper.Instance.Error(LogHelper.commonService,"ss2sasdasdasdas");

            ////CopyLogConfig copyLogConfig = new CopyLogConfig();

            ////using (ConfigDAO configDAO = new ConfigDAO())
            ////{
            ////    copyLogConfig = configDAO.GetConfig();
            ////}
            ////StartupScheduleJob();

            //UserIPList result = null;
            //    UserIPCriteria criteria = new UserIPCriteria();
            //    criteria.PageSize = 8;
            //    criteria.PageIndex = 1;
            //    criteria.SearchColumn = "user_name";
            //    criteria.SearchText = "6666a";
            //UserIPMapService service = new UserIPMapService();
            //result = service.Search(criteria);

            //UserIPMapService service = new UserIPMapService();
            //service.Delete(1);

            //UserIPMapService service = new UserIPMapService();

            //service.AddOrUpdate(new UserIPInfo() { SID = 1, IPAddress="192.168.184.27", UserName = "ashers"});
            //service.IsExist("192.168.184.287");

            //using (RedisClient redisClient = RedisHelper.Instance)
            //{
            //    redisClient.Add("testt1","111");

            //    string s = redisClient.Get<string>("testt1");
            //}
            //List<string> list = new List<string>();
            //list.Add("www.baidu.com");
            //list.Add("www.baidsuss.com");
            //list.Add("www.youku.com");
            //list.Add("www.sina.com");
            //list.Add("www.qidian.com");
            //list.Add("www.iqiyi.com");
            //list.Add("www.douyutv8.com");

            //IPMonitorHelper.PushIPStack(list);
            //IPMonitorHelper.StopMonitorThread();
            //IPMonitorHelper.StartMonitorThread();

            //Thread.Sleep(60000);

            //LocalIPStatus status = RedisHelper.GetIPStatus("www.baidu.com");
            //LocalIPStatus status2 = RedisHelper.GetIPStatus("www.baidsuss.com");
            //LocalIPStatus status3 = RedisHelper.GetIPStatus("www.youku.com");
            //LocalIPStatus status4 = RedisHelper.GetIPStatus("www.sina.com");
            //LocalIPStatus status5 = RedisHelper.GetIPStatus("www.qidian.com");
            //LocalIPStatus status6 = RedisHelper.GetIPStatus("www.iqiyi.com");
            //LocalIPStatus status7 = RedisHelper.GetIPStatus("www.douyutv8.com");

            //List<BrefAlertInfo> l2 = IPMonitorHelper.GetAlertInfo();
            //List<BrefIPInfo> l3 = IPMonitorHelper.GetRecord();

            //List<BrefIPInfo> brefIpInfoList = new List<BrefIPInfo>();
            //brefIpInfoList.Add(new BrefIPInfo() { IP = "1", LostTime = DateTime.Now, RecoveryTime = DateTime.Now });
            //brefIpInfoList.Add(new BrefIPInfo() { IP = "2", LostTime = DateTime.Now, RecoveryTime = DateTime.Now });
            //brefIpInfoList.Add(new BrefIPInfo() { IP = "3", LostTime = DateTime.Now, RecoveryTime = DateTime.Now });

            //new IPMonitorDAO().LoadMonitorRecord(l3);
            //new AlertDAO().LoadAlertRecord(l2);

            //List<IPRegionPair> list4 = new IPMonitorService().GetIpListForMonitor();

            //IPMonitorListModel r1 = new IPMonitorDAO().GetIPRegionList(
            //    new IPRegionListCriteria()
            //    {
            //        SearchColumn = "ip",
            //        SearchText = "",
            //        PageSize = 8,
            //        PageIndex = 1,
            //        Region = ""
            //    });

            //new IPMonitorDAO().AddOrUpdateIPRegion(new BrefIPRegionInfo() { IP = "192.168.184.1", Model = "123aw", Region = "XiFu"});

            //new IPMonitorDAO().DeleteIPRegion(7);

            //new IPMonitorDAO().EditIPRegion(6);

            //new IPMonitorDAO().GetAlertInfo(new AlertInfoCriteria()
            //{
            //    SearchColumn = "",
            //    SearchText = "",
            //    FromDate = DateTime.Parse("2015-01-01"),
            //    ToDate = DateTime.Parse("2016-01-01"),
            //    IsSend = false,
            //    PageIndex = 1,
            //    PageSize = 8
            //});

            //new IPMonitorDAO().GetMonitorRecord(new MonitorRecordCriteria()
            //{
            //    SearchColumn = "",
            //    SearchText = "",
            //    Region = "NingWu",
            //    FromDate = DateTime.Parse("2015-01-01"),
            //    ToDate = DateTime.Parse("2016-01-01"),
            //    PageIndex = 1,
            //    PageSize = 8
            //});

            //UserIPList ss = new UserIPDAO().Search(new UserIPCriteria()
            //{
            //    SearchColumn = "",
            //    SearchText = "",
            //    PageIndex = 1,
            //    PageSize = 8
            //});
            //List<IPRegionPair> list = new IPMonitorService().GetIPRegionStatusPair(new IPRegionListCriteria()
            //{
            //    SearchColumn = "",
            //    SearchText = "",
            //    Region = "",
            //    PageIndex = 1,
            //    PageSize = 8
            //});

            //bool s4 = new IPMonitorService().IsExist(11, "www.baidu.com2");
            //bool s4 = new UserIPMapService().IsExist(0, "192.168.184.5");
            //string a = MD5Customer.MD5Encrypt("1111111111", "Monitor?");
            //string ss = MD5Customer.MD5Decrypt(a, "Monitor?");

            //new UserService().CheckUserNameAndPassword(new BrefUserInfo() { UserName = "admin", Password = "AAFB33BD5EDB669A" });

            //string a = MD5Customer.MD5Encrypt("xrs123!@#", "Monitor?");
            //new UserService().ChangePasswd(new UserInfo() { SID = 1, NewPassword = a});

            ////Test LogInfoGuide
            //LogInfoService service = new LogInfoService();

            //List<LogLevelGuide> result = service.GetLogLevelGuideList();

            //LogInfoGuideList list = service.SearchLogInfoGuideList(new LogInfoGuideCriteria()
            //{
            //    SearchColumn = "keywords",
            //    SearchText = "",
            //    PageIndex = 1,
            //    PageSize = 10
            //});
        }
    }
}

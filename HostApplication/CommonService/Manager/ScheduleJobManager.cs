using CommonConst;
using CommonService.ScheduleJob;
using DataAccess.DAO;
using DataAccess.Model;
using PlatForm.Util;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonService.Manager
{
    public class ScheduleJobManager
    {
        public IScheduler scheduleJob = StdSchedulerFactory.GetDefaultScheduler();

        private static ScheduleJobManager instance = new ScheduleJobManager();

        private ScheduleJobManager()
        {
        }

        public static ScheduleJobManager Instance
        {

            get
            {
                return instance;
            }
        }

        public void StartCopyLogInfo()
        {
            CopyLogConfig copyLogConfig = new CopyLogConfig();

            using (ConfigDAO configDAO = new ConfigDAO())
            {
                copyLogConfig = configDAO.GetConfig();
            }

            if (null == copyLogConfig)
            {
                LogHelper.Instance.Error(LogHelper.CommonService, "CopyLogConfig is null");
                throw new Exception("Config file error");
            }

            IJobDetail job = JobBuilder.Create<CopyLogIntoDB>()
                   .WithIdentity(copyLogConfig.JobName, copyLogConfig.JobGroup)
                   .Build();

            ITrigger trigger = null;

            trigger = TriggerBuilder.Create().WithIdentity(copyLogConfig.JobName, copyLogConfig.JobGroup)
                .WithSimpleSchedule(x => x.WithIntervalInHours(int.Parse(copyLogConfig.Corn)).RepeatForever()).Build();

            job.JobDataMap[ScheduleJobConst.StartupPath] = copyLogConfig.StartupPath;
            job.JobDataMap[ScheduleJobConst.CustomDB] = copyLogConfig.CustomDB;
            job.JobDataMap[ScheduleJobConst.LogPath] = copyLogConfig.LogPath;
            job.JobDataMap[ScheduleJobConst.TableStruct] = copyLogConfig.TableStruct;
            job.JobDataMap[ScheduleJobConst.DateFormatInfoDic] = copyLogConfig.DateFormatInfoDic;

            scheduleJob.ScheduleJob(job, trigger);
        }

        public void StartMonitorIPJob()
        {
            IJobDetail job = JobBuilder.Create<LoadMonitorInfo>()
                   .WithIdentity("MonitorIP", "MonitorIP")
                   .Build();

            ITrigger trigger = TriggerBuilder.Create().WithIdentity("MonitorIP", "MonitorIP")
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(10).RepeatForever()).Build();

            scheduleJob.ScheduleJob(job, trigger);
        }

        public void StartSendMessageJob()
        {
            IJobDetail job = JobBuilder.Create<SendMessageJob>()
                   .WithIdentity("SendMessageJob", "SendMessageJob")
                   .Build();

            ITrigger trigger = TriggerBuilder.Create().WithIdentity("SendMessageJob", "SendMessageJob")
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(12).RepeatForever()).Build();

            scheduleJob.ScheduleJob(job, trigger);
        }

        public void Start()
        {
            scheduleJob.Start();
        }

        public void Clear()
        {
            scheduleJob.Clear();
        }
    }
}

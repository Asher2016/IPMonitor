using CommonContract.Service;
using CommonService.Manager;
using CommonService.Service;
using DataAccess.DAO;
using DataAccess.Model;
using PlatForm.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace HostApplication
{
    public partial class HostedApplicationServer : ServiceBase
    {
        List<ServiceHost> serviceList = new List<ServiceHost>();

        public HostedApplicationServer()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            StartupScheduleJob();
            StartMonitorIPJob();
            StartSendMessageJob();
            ScheduleJobManager.Instance.Start();

            serviceList.Add(new ServiceHost(typeof(LogInfoService)));
            serviceList.Add(new ServiceHost(typeof(UserService)));
            serviceList.Add(new ServiceHost(typeof(UserIPMapService)));
            serviceList.Add(new ServiceHost(typeof(IPMonitorService)));

            foreach (ServiceHost service in serviceList)
            {
                service.Open();
            }
        }

        protected override void OnStop()
        {
            foreach (ServiceHost host in serviceList)
            {
                host.Close();
            }

            serviceList.Clear();

            ScheduleJobManager.Instance.Clear();
        }

        public void StartupScheduleJob()
        {
            try
            {
                ScheduleJobManager.Instance.StartCopyLogInfo();
            }
            catch(Exception exception)
            {
                LogHelper.Instance.Error(LogHelper.CommonService,
                    string.Format("StartupScheduleJob fail ~~, Message {0}.", exception.Message));
            }
        }

        public void StartMonitorIPJob()
        {
            try
            {
                ScheduleJobManager.Instance.StartMonitorIPJob();
            }
            catch (Exception exception)
            {
                LogHelper.Instance.Error(LogHelper.CommonService,
                    string.Format("StartMonitorIPJob fail ~~, Message {0}.", exception.Message));
            }
        }

        public void StartSendMessageJob()
        {
            try
            {
                ScheduleJobManager.Instance.StartSendMessageJob();
            }
            catch (Exception exception)
            {
                LogHelper.Instance.Error(LogHelper.CommonService,
                        string.Format("StartSendMessageJob fail ~~, Message {0}.", exception.Message));
            }
        }
    }
}

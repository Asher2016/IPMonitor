using CommonConst;
using CommonContract.Model;
using CommonContract.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using WebApplication.Util;

namespace WebApplication.Manager
{
    public class IPMonitorManager : IIPMonitorService
    {
        WcfClient<IIPMonitorService> client = new WcfClient<IIPMonitorService>();
        IIPMonitorService service = null;

        public IPMonitorManager()
        {
            try
            {
                service = client.GetService("IIPMonitorService");
            }
            catch (FaultException exception)
            {
                LogHelper.Instance.Error(LogHelper.WebSite, exception.Message);
                throw new Exception("Internal Server Error.", exception.InnerException);
            }
        }

        public List<IPRegionPair> GetIPRegionStatusPair(IPRegionListCriteria criteria)
        {
            List<IPRegionPair> result = null;

            try
            {
                result = service.GetIPRegionStatusPair(criteria);
            }
            catch (FaultException exception)
            {
                LogHelper.Instance.Error(LogHelper.WebSite, exception.Message);
                throw new Exception("Internal Server Error.", exception.InnerException);
            }

            return result;
        }

        public IPMonitorListModel GetIPRegionList(IPRegionListCriteria criteria)
        {
            IPMonitorListModel resutlModel = new IPMonitorListModel();

            try
            {
                resutlModel= service.GetIPRegionList(criteria);
            }
            catch (FaultException exception)
            {
                LogHelper.Instance.Error(LogHelper.WebSite, exception.Message);
                throw new Exception("Internal Server Error.", exception.InnerException);
            }

            return resutlModel;
        }

        public void AddOrUpdateIPRegion(BrefIPRegionInfo brefIPRegionInfo)
        {
            try
            {
                service.AddOrUpdateIPRegion(brefIPRegionInfo);
            }
            catch (FaultException exception)
            {
                LogHelper.Instance.Error(LogHelper.WebSite, exception.Message);
                throw new Exception("Internal Server Error.", exception.InnerException);
            }
        }

        public void DeleteIPRegion(long sid)
        {
            try
            {
                service.DeleteIPRegion(sid);
            }
            catch (FaultException exception)
            {
                LogHelper.Instance.Error(LogHelper.WebSite, exception.Message);
                throw new Exception("Internal Server Error.", exception.InnerException);
            }
        }

        public MonitorRecordListModel GetMonitorRecord(MonitorRecordCriteria criteria)
        {
            MonitorRecordListModel resultModel = new MonitorRecordListModel();

            try
            {
                resultModel = service.GetMonitorRecord(criteria);
            }
            catch (FaultException exception)
            {
                LogHelper.Instance.Error(LogHelper.WebSite, exception.Message);
                throw new Exception("Internal Server Error.", exception.InnerException);
            }

            return resultModel;
        }

        public AlertInfoListModel GetAlertInfo(AlertInfoCriteria criteria)
        {
            AlertInfoListModel resultModel = new AlertInfoListModel();

            try
            {
                resultModel = service.GetAlertInfo(criteria);
            }
            catch (FaultException exception)
            {
                LogHelper.Instance.Error(LogHelper.WebSite, exception.Message);
                throw new Exception("Internal Server Error.", exception.InnerException);
            }

            return resultModel;
        }

        public BrefIPRegionInfo EditIPRegion(long sid)
        {
            BrefIPRegionInfo result = null;
            try
            {
                result = service.EditIPRegion(sid);
            }
            catch (FaultException exception)
            {
                LogHelper.Instance.Error(LogHelper.WebSite, exception.Message);
                throw new Exception("Internal Server Error.", exception.InnerException);
            }

            return result;
        }

        public List<IPRegionPair> GetAllIpListStatus()
        {
            List<IPRegionPair> result = null;

            try
            {
                result = service.GetAllIpListStatus();
            }
            catch (FaultException exception)
            {
                LogHelper.Instance.Error(LogHelper.WebSite, exception.Message);
                throw new Exception("Internal Server Error.", exception.InnerException);
            }

            return result;
        }

        public bool IsExist(long sid, string ip)
        {
            bool result = false;

            try
            {
                result = service.IsExist(sid, ip);
            }
            catch (FaultException exception)
            {
                LogHelper.Instance.Error(LogHelper.WebSite, exception.Message);
                throw new Exception("Internal Server Error.", exception.InnerException);
            }

            return result;
        }
    }
}
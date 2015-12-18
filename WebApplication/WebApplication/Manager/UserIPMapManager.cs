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
    public class UserIPMapManager : IUserIPMapService
    {
        WcfClient<IUserIPMapService> client = new WcfClient<IUserIPMapService>();
        IUserIPMapService service = null;

        public UserIPMapManager()
        {
            try
            {
                service = client.GetService("IUserIPMapService");
            }
            catch (FaultException exception)
            {
                LogHelper.Instance.Error(LogHelper.WebSite, exception.Message);
            }
        }

        public UserIPList Search(UserIPCriteria criteria)
        {
            UserIPList list = null;

            try
            {
                list = service.Search(criteria);
            }
            catch (FaultException exception)
            {
                LogHelper.Instance.Error(LogHelper.WebSite, exception.Message);
                throw new Exception("Internal Server Error.", exception.InnerException);
            }

            return list;
        }

        public void Delete(long sid)
        {
            try
            {
                service.Delete(sid);
            }
            catch (FaultException exception)
            {
                LogHelper.Instance.Error(LogHelper.WebSite, exception.Message);
                throw new Exception("Internal Server Error.", exception.InnerException);
            }
        }

        public UserIPInfo Edit(long sid)
        {
            UserIPInfo userIPInfo = null;

            try
            {
                userIPInfo = service.Edit(sid);
            }
            catch (FaultException exception)
            {
                LogHelper.Instance.Error(LogHelper.WebSite, exception.Message);
                throw new Exception("Internal Server Error.", exception.InnerException);
            }

            return userIPInfo;
        }

        public void AddOrUpdate(UserIPInfo userIPInfo)
        {
            try
            {
                service.AddOrUpdate(userIPInfo);
            }
            catch (FaultException exception)
            {
                LogHelper.Instance.Error(LogHelper.WebSite, exception.Message);
                throw new Exception("Internal Server Error.", exception.InnerException);
            }
        }

        public bool IsExist(long sid, string ipAddress)
        {
            bool result = false;

            try
            {
                result = service.IsExist(sid, ipAddress);
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
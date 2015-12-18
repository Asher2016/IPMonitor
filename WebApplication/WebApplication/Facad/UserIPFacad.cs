using CommonContract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication.Manager;

namespace WebApplication.Facad
{
    public class UserIPFacad
    {
        UserIPMapManager manager = new UserIPMapManager();

        public UserIPList Search(UserIPCriteria criteria)
        {
            UserIPList result = null;

            try
            {
                result = manager.Search(criteria);
            }
            catch(Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return result;
        }

        public UserIPInfo Edit(long sid)
        {
            UserIPInfo result = null;

            try
            {
                result = manager.Edit(sid);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return result;
        }

        public void AddOrUpdate(UserIPInfo userIPInfo)
        {
            try
            {
                manager.AddOrUpdate(userIPInfo);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public void Delete(long sid)
        {
            try
            {
                manager.Delete(sid);
            }
            catch(Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public bool IsExist(long sid, string ip)
        {
            bool result = false;

            try
            {
                result = manager.IsExist(sid, ip);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return result;
        }
    }
}
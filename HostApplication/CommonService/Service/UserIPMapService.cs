using CommonContract.Model;
using CommonContract.Service;
using DataAccess.DAO;
using PlatForm.Util;
using System;
using System.ServiceModel;

namespace CommonService.Service
{
    public class UserIPMapService : IUserIPMapService
    {
        public UserIPList Search(UserIPCriteria criteria)
        {
            UserIPList result = null;

            try
            {
                using (UserIPDAO dao = new UserIPDAO())
                {
                    result = dao.Search(criteria);
                }
            }
            catch(Exception e)
            {
                LogHelper.Instance.Error(LogHelper.CommonService, "execute ods.ip_user_map_search error " + e.Message);
                throw new FaultException("Internal Server Error");
            }

            return result;
        }

        public void Delete(long sid)
        {
            try
            {
                using (UserIPDAO dao = new UserIPDAO())
                {
                    dao.Delete(sid);
                }
            }
            catch (Exception e)
            {
                LogHelper.Instance.Error(LogHelper.CommonService, "execute delete user ip map error " + e.Message);
                throw new FaultException("Internal Server Error");
            }
        }

        public UserIPInfo Edit(long sid)
        {
            UserIPInfo result = null;

            try
            {
                using (UserIPDAO dao = new UserIPDAO())
                {
                    result = dao.Edit(sid);
                }
            }
            catch (Exception e)
            {
                LogHelper.Instance.Error(LogHelper.CommonService, "execute edit user ip map error " + e.Message);
                throw new FaultException("Internal Server Error");
            }

            return result;
        }

        public void AddOrUpdate(UserIPInfo userIPInfo)
        {
            try
            {
                using (UserIPDAO dao = new UserIPDAO())
                {
                    dao.AddOrUpdate(userIPInfo);
                }
            }
            catch (Exception e)
            {
                LogHelper.Instance.Error(LogHelper.CommonService, "execute AddOrUpdate user ip map error " + e.Message);
                throw new FaultException("Internal Server Error");
            }
        }

        public bool IsExist(long sid, string ipAddress)
        {
            bool result = false;

            try
            {
                using (UserIPDAO dao = new UserIPDAO())
                {
                    result = dao.IsExist(sid, ipAddress);
                }
            }
            catch (Exception e)
            {
                LogHelper.Instance.Error(LogHelper.CommonService, "execute IsExist user ip map error " + e.Message);
                throw new FaultException("Internal Server Error");
            }

            return result;
        }
    }
}

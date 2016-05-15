using CommonContract.Model;
using CommonContract.Service;
using DataAccess.DAO;
using System;
using System.ServiceModel;

namespace CommonService.Service
{
    public class UserService : IUserService
    {
       public BrefUserInfo CheckUserNameAndPassword(BrefUserInfo brefUserInfo)
        {
            BrefUserInfo result = null;

            try
            {
                using (UserDAO dao = new UserDAO())
                {
                    result = dao.CheckUserNameAndPassword(brefUserInfo);
                }
            }
            catch
            {
                throw new FaultException("Execute CheckUserNameAndPassword error.", new FaultCode("CheckUserNameAndPassword"));
            }

            return result;
        }

        public void ChangePasswd(UserInfo userInfo)
        {
            try
            {
                using (UserDAO dao = new UserDAO())
                {
                    dao.ChangePasswd(userInfo);
                }
            }
            catch
            {
                throw new FaultException("Execute ChangePasswd error.", new FaultCode("ChangePasswd"));
            }
        }

        public bool CheckOldPasswd(UserInfo userInfo)
        {
            bool result = false;

            try
            {
                using (UserDAO dao = new UserDAO())
                {
                    result = dao.CheckOldPasswd(userInfo);
                }
            }
            catch
            {
                throw new FaultException("Execute CheckOldPasswd error.", new FaultCode("CheckOldPasswd"));
            }

            return result;
        }
    }
}

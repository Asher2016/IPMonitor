using CommonContract.Model;
using CommonContract.Service;
using System;
using System.ServiceModel;
using WebApplication.Util;

namespace WebApplication.Manager
{
    public class UserManager:IUserService
    {
        WcfClient<IUserService> client = new WcfClient<IUserService>();
        IUserService service = null;

        public UserManager()
        {
            service = client.GetService("IUserService");
        }

        public BrefUserInfo CheckUserNameAndPassword(BrefUserInfo brefUserInfo)
        {
            BrefUserInfo result = new BrefUserInfo();

            try
            {
                brefUserInfo.Password = MD5Customer.MD5Encrypt(brefUserInfo.Password, MD5Customer.MD5Key);
                result = service.CheckUserNameAndPassword(brefUserInfo);
            }
            catch (FaultException exception)
            {
                LogHelper.Instance.Error(LogHelper.WebSite, exception.Message);
                throw new Exception("Internal Server Error.", exception.InnerException);
            }

            return result;
        }

        public void ChangePasswd(UserInfo userInfo)
        {
            try
            {
                userInfo.NewPassword = MD5Customer.MD5Encrypt(userInfo.NewPassword, MD5Customer.MD5Key);
                service.ChangePasswd(userInfo);
            }
            catch (FaultException exception)
            {
                LogHelper.Instance.Error(LogHelper.WebSite, exception.Message);
                throw new Exception("Internal Server Error.", exception.InnerException);
            }
        }

        public bool CheckOldPasswd(UserInfo userInfo)
        {
            bool result = false;

            try
            {
                userInfo.OldPassword = MD5Customer.MD5Encrypt(userInfo.OldPassword, MD5Customer.MD5Key);
                result = service.CheckOldPasswd(userInfo);
                userInfo.OldPassword = MD5Customer.MD5Decrypt(userInfo.OldPassword, MD5Customer.MD5Key);
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
using CommonContract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication.Manager;

namespace WebApplication.Facad
{
    public class UserFacad
    {
        UserManager manager = new UserManager();

        public BrefUserInfo CheckUserNameAndPassword(BrefUserInfo brefUserInfo)
        {
            BrefUserInfo result = new BrefUserInfo();

            try
            {
                result = manager.CheckUserNameAndPassword(brefUserInfo);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return result;
        }

        public void ChangePasswd(UserInfo userInfo)
        {
            try
            {
                manager.ChangePasswd(userInfo);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public bool CheckOldPasswd(UserInfo userInfo)
        {
            bool result = false;

            try
            {
                result = manager.CheckOldPasswd(userInfo);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return result;
        }
    }
}
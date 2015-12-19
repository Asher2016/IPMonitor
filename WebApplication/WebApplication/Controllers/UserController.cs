using CommonContract.Model;
using System.Net;
using System.Web.Mvc;
using WebApplication.Facad;
using WebApplication.Manager;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class UserController : Controller
    {
        UserFacad facad = new UserFacad();
        //
        // GET: /User/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoginView()
        {
            LoginViewModel loginViewModel = new LoginViewModel();
            loginViewModel.UserName = string.Empty;
            loginViewModel.Password = string.Empty;

            return PartialView(loginViewModel);
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    BrefUserInfo user = facad.CheckUserNameAndPassword(new BrefUserInfo() { UserName = loginViewModel.UserName, Password = loginViewModel.Password });

                    if (user != null)
                    {
                        Session.Add("UserInfo", user);
                        return null;
                    }
                    else
                    {
                        ModelState.AddModelError("LoginViewModel.UserName", "用户名密码错误");
                        return PartialView("LoginView", loginViewModel);
                    }
                }
                catch
                {
                    ModelState.AddModelError("LoginViewModel.UserName", "Internal Server Error.");
                }
            }

            return PartialView("LoginView", loginViewModel);
        }

        public ActionResult GetChangePasswdView()
        {
            BrefUserInfo userInfo = new BrefUserInfo();
            UserInfo user = new UserInfo();

            try
            {
                userInfo = (BrefUserInfo)Session["UserInfo"];
                user.UserName = userInfo.UserName;
                user.SID = userInfo.SID;
            }
            catch
            {
                return Redirect("../Home/Index");
            }

            return PartialView("ChangePassword", user);
        }

        public ActionResult ChangePasswd(UserInfo userInfo)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("ChangePassword", userInfo);
            }

            ValidatePasswd(userInfo);

            if (ModelState.IsValid)
            {
                try
                {
                    facad.ChangePasswd(userInfo);
                    return null;
                }
                catch
                {
                    ModelState.AddModelError("UserInfo.UserName", "Internal Server Error.");
                    return PartialView("ChangePassword", userInfo);
                }
            }

            return PartialView("ChangePassword", userInfo);
        }

        private void ValidatePasswd(UserInfo userInfo)
        {
            if (!userInfo.NewPassword.Equals(userInfo.ConfirmPassword))
            {
                ModelState.AddModelError("UserInfo.NewPassword", "两次密码输入不一致");
            }

            if (!facad.CheckOldPasswd(userInfo))
            {
                ModelState.AddModelError("UserInfo.OldPassword", "旧密码错误");
            }
        }

        public bool CheckSession()
        {
            if (null == Session["UserInfo"])
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool Exit()
        {
            bool result = false;

            Session.Remove("UserInfo");
            result = true;

            return result;
        }
	}
}
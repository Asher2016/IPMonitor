using CommonContract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication.Facad;
using WebApplication.Manager;

namespace WebApplication.Controllers
{
    public class UserIPMapController : Controller
    {
        UserIPFacad facad = new UserIPFacad();

        //
        // GET: /UserIPMap/
        public ActionResult ListView()
        {
            return PartialView();
        }

        public ActionResult CreateView()
        {
            UserIPInfo userIPInfo = new UserIPInfo();
            userIPInfo.SID = -1;
            return PartialView("CreateOrEdit", userIPInfo);
        }

        public JsonResult Search(UserIPCriteria criteria)
        {
            UserIPList list = facad.Search(criteria);

            if (list.Count == 0)
            {
                list.Count = 1;
            }

            return Json(list);
        }

        public ActionResult Edit(long sid)
        {
            UserIPInfo userIPInfo = facad.Edit(sid);

            return PartialView("CreateOrEdit", userIPInfo);
        }

        public void Delete(long sid)
        {
            facad.Delete(sid);
        }

        [HttpPost]
        public ActionResult AddOrUpdate(UserIPInfo userIPInfo)
        {
            Valicate(userIPInfo);

            if (ModelState.IsValid)
            {
                facad.AddOrUpdate(userIPInfo);
            }
            else
            {
                return PartialView("CreateOrEdit", userIPInfo);
            }

            return null;
        }

        private void Valicate(UserIPInfo userIPInfo)
        {
            IPAddress temp;
            if (string.IsNullOrEmpty(userIPInfo.IPAddress))
            {
                ModelState.AddModelError("IPAddress", "IP地址是必填字段.");
            }
            else if (!IPAddress.TryParse(userIPInfo.IPAddress, out temp))
            {
                ModelState.AddModelError("IPAddress", "IP地址格式错误.");
            }

            if (string.IsNullOrEmpty(userIPInfo.UserName))
            {
                ModelState.AddModelError("UserName", "匹配用户不能为空.");
            }

            if (facad.IsExist(userIPInfo.SID, userIPInfo.IPAddress))
            {
                ModelState.AddModelError("IPAddress", "此IP存在匹配关系.");
            }
        }
	}
}
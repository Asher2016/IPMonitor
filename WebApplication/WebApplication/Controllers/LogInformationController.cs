using CommonContract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Facad;
using WebApplication.Manager;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class LogInformationController : Controller
    {
        LogInformationFacad facad = new LogInformationFacad();
        //
        // GET: /LogInformation/
        public ActionResult ClientListView()
        {
            return PartialView("ClientLogInformationListView");
        }

        public ActionResult InterchangerListView()
        {
            return PartialView("InterchangerLogInformationListView");
        }

        [HttpPost]
        public JsonResult SearchLogInfo(LogCriteria criteria)
        {
             LogListContract list = facad.SearchLogInfo(criteria);

            return Json(list);
        }

        public ActionResult LogLevelGuideListView()
        {
            return PartialView("LogLevelGuideListView");
        }

        [HttpPost]
        public JsonResult GetLogLevelGuideList()
        {
            List<LogLevelGuide> list = facad.GetLogLevelGuideList();

            return Json(list);
        }

        public ActionResult LogInfoGuideListView()
        {
            return PartialView("LogInfoGuideListView");
        }

        [HttpPost]
        public JsonResult SearchLogInfoGuideList(LogInfoGuideCriteria criteria)
        {
            LogInfoGuideList list = facad.SearchLogInfoGuideList(criteria);

            return Json(list);
        }
	}
}
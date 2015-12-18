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
        public ActionResult ListView()
        {
            return PartialView("LogInformationListView");
        }

        [HttpPost]
        public JsonResult SearchLogInfo(LogCriteria criteria)
        {
             LogListContract list = facad.SearchLogInfo(criteria);

            return Json(list);
        }
	}
}
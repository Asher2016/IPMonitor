using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class IPController : Controller
    {
        //
        // GET: /IP/
        public ActionResult ViewList()
        {
            return PartialView();
        }

        public JsonResult DataList(string searchColumn, string searchText, int pageIndex)
        {
            return Json(null);
        }

        public void DeleteItem(string searchColumn, string searchText, int pageIndex, string sid)
        {
            //delete
            DataList(searchColumn, searchText, pageIndex);
        }

        public ActionResult EditItemView(int sid)
        {
            return PartialView("CreateOrEdit");
        }

        public JsonResult EditItem()
        {
            return DataList2("", "", 1);
        }

        public JsonResult DataList2(string searchColumn, string searchText, int pageIndex)
        {
            return Json(null);
        }
	}
}
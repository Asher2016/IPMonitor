using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommonContract.Model;
using WebApplication.Facad;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        IPMonitorFacad facad = new IPMonitorFacad();
        public static int index = 1;
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }
	}
}
using CommonContract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebApplication.Facad;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class IPMonitorController : Controller
    {
        IPMonitorFacad facad = new IPMonitorFacad();

        // GET: /IPMonitor/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListView()
        {
            return PartialView("IPMonitorListView");
        }

        public JsonResult GetIPRegionList(IPRegionListCriteria criteria)
        {
            IPMonitorListModel result = null;
            result = facad.GetIPRegionList(criteria);

            if (result.Count == 0)
            {
                result.Count = 1;
            }

            return Json(result);
        }

        public ActionResult CreateView()
        {
            BrefIPRegionInfo brefRegionInfo = new BrefIPRegionInfo();
            brefRegionInfo.Region = "XiFuQu";
            return PartialView("IPMonitorCreateOrUpdate", brefRegionInfo);
        }

        public ActionResult Edit(long sid)
        {
            BrefIPRegionInfo userIPInfo = facad.EditIPRegion(sid);

            return PartialView("IPMonitorCreateOrUpdate", userIPInfo);
        }

        public void Delete(long sid)
        {
            facad.DeleteIPRegion(sid);
        }

        public ActionResult AddOrUpdate(BrefIPRegionInfo brefInfo)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("IPMonitorCreateOrUpdate", brefInfo);
            }

            Validate(brefInfo);

            if (ModelState.IsValid)
            {
                facad.AddOrUpdateMonitorRecord(brefInfo);
            }
            else
            {
                return PartialView("IPMonitorCreateOrUpdate", brefInfo);
            }

            return null;
        }

        public void Validate(BrefIPRegionInfo brefInfo)
        {
            //IPAddress temp;
            //if (!IPAddress.TryParse(brefInfo.IP, out temp))
            //{
            //    ModelState.AddModelError("IP", "IP地址格式错误.");
            //}

            if (facad.IsExist(brefInfo.SID, brefInfo.IP))
            {
                ModelState.AddModelError("IP", "此IP地址已被监听.");
            }

            if (!string.IsNullOrWhiteSpace(brefInfo.Telephone)){
                string[] telephoneArray = brefInfo.Telephone.Trim().Split(',');

                foreach (string item in telephoneArray)
                {
                    string tempItem = item.Trim();
                    if (!Regex.IsMatch(tempItem, @"^1\d{10}$"))
                    {
                        ModelState.AddModelError("Telephone", tempItem + " 电话格式不正确.");
                    }
                }
            }
        }

        public ActionResult MonitorRecoedView()
        {
            return PartialView("IPMonitorRecordList");
        }

        public ActionResult AlertInfoView()
        {
            return PartialView("AlertInfoList");
        }

        public JsonResult GetMonitorRecord(MonitorRecordCriteria criteria)
        {
            MonitorRecordListViewModel result = null;
            result = facad.GetMonitorRecord(criteria);

            if (result.Count == 0)
            {
                result.Count = 1;
            }

            return Json(result);
        }

        public JsonResult GetAlertInfo(AlertInfoCriteria criteria)
        {
            AlertInfoListViewModel result = null;
            result = facad.GetAlertInfo(criteria);

            if (result.Count == 0)
            {
                result.Count = 1;
            }

            return Json(result);
        }

        public JsonResult GetAllRegionStatus()
        {
            List<IPRegionPairView> result = facad.GetAllRegionStatus();

            return Json(result);
        }

        public JsonResult GetIPMonitorListStatus(IPRegionListCriteria criteria)
        {
            List<IPRegionPairView> result = facad.GetIPMonitorListStatus(criteria);

            return Json(result);
        }

        public ActionResult GetIPMonitorBrefInfoListView(string region)
        {
            if (null == Session["UserInfo"])
            {
                return null;
            }

            MonitorRecordCriteriaView criteria = new MonitorRecordCriteriaView();
            criteria.Region = region;

            switch (region)
            {
                case "PianGuan":
                        criteria.DisplayRegion = "偏关县";
                        break;
                case "HeQu":
                        criteria.DisplayRegion = "河曲县";
                        break;
                case "BaoDe":
                        criteria.DisplayRegion = "保德县";
                        break;
                case "ShenChi":
                        criteria.DisplayRegion = "神池县";
                        break;
                case "WuZhai":
                        criteria.DisplayRegion = "五寨县";
                        break;
                case "KeLan":
                        criteria.DisplayRegion = "岢岚县";
                        break;
                case "NingWu":
                        criteria.DisplayRegion = "宁武县";
                        break;
                case "JingLe":
                        criteria.DisplayRegion = "静乐县";
                        break;
                case "YuanPing":
                        criteria.DisplayRegion = "原平县";
                        break;
                case "XiFu":
                        criteria.DisplayRegion = "忻府区";
                        break;
                case "Dai":
                        criteria.DisplayRegion = "代县";
                        break;
                case "WuTai":
                        criteria.DisplayRegion = "五台县";
                        break;
                case "DingXiang":
                        criteria.DisplayRegion = "定襄县";
                        break;
                case "FanZhi":
                        criteria.DisplayRegion = "繁峙县";
                        break;
            }

            return PartialView("IPMonitorBrefInfoList", criteria);
        }
	}
}
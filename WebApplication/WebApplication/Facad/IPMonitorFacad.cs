using CommonContract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication.Manager;
using WebApplication.Models;
using WebApplication.Util;

namespace WebApplication.Facad
{
    public class IPMonitorFacad
    {
        IPMonitorManager manager = new IPMonitorManager();

        public IPMonitorListModel GetIPRegionList(IPRegionListCriteria criteria)
        {
            IPMonitorListModel result = null;

            try
            {
                result = manager.GetIPRegionList(criteria);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return result;
        }

        public BrefIPRegionInfo EditIPRegion(long sid)
        {
            BrefIPRegionInfo result;

            try
            {
                result = manager.EditIPRegion(sid);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return result;
        }

        public void DeleteIPRegion(long sid)
        {
            try
            {
                manager.DeleteIPRegion(sid);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

        }

        public void AddOrUpdateMonitorRecord(BrefIPRegionInfo brefInfo)
        {
            try
            {
                manager.AddOrUpdateIPRegion(brefInfo);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public MonitorRecordListViewModel GetMonitorRecord(MonitorRecordCriteria criteria)
        {
            MonitorRecordListModel result = null;

            try
            {
                result = manager.GetMonitorRecord(criteria);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return EntityToViewModel(result);
        }

        public MonitorRecordListViewModel EntityToViewModel(MonitorRecordListModel entity)
        {
            MonitorRecordListViewModel result = new MonitorRecordListViewModel();
            result.Count = entity.Count;
            List<BrefIPInfoView> list = new List<BrefIPInfoView>();

            foreach(BrefIPInfo item in entity.BrefIPInfoList)
            {
                list.Add(new BrefIPInfoView()
                    {
                        IP = item.IP,
                        Region = item.Region,
                        Model = item.Model,
                        LostTime = item.LostTime == null ? "" : item.LostTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        RecoveryTime = item.RecoveryTime == null ? "" : item.RecoveryTime == DateTime.MinValue ? "未恢复" : item.RecoveryTime.ToString("yyyy-MM-dd HH:mm:ss")
                    });
            }
            result.BrefIPInfoList = list;
            return result;
        }
        public AlertInfoListViewModel GetAlertInfo(AlertInfoCriteria criteria)
        {
            AlertInfoListModel result = null;

            try
            {
                result = manager.GetAlertInfo(criteria);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            if (result.Count == 0)
            {
                result.Count = 1;
            }

            return EntityToViewModel(result);
        }

        public AlertInfoListViewModel EntityToViewModel(AlertInfoListModel entity)
        {
            AlertInfoListViewModel result = new AlertInfoListViewModel();
            result.Count = entity.Count;

            List<BrefAlertInfoView> list = new List<BrefAlertInfoView>();

            foreach(BrefAlertInfo item in entity.BrefAlertInfoList)
            {
                list.Add(new BrefAlertInfoView()
                    {
                        IP = item.IP,
                        Region = item.Region,
                        Model = item.Model,
                        IsSend = item.IsSend == true ? "已发" : "未发",
                        FirstLostTime = item.FirstLostTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        SecondLostTime = item.SecondLostTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        RecoveryTime = item.RecoveryTime == DateTime.MinValue ? "未恢复" : item.RecoveryTime.ToString("yyyy-MM-dd HH:mm:ss")
                    });
            }
            result.BrefAlertInfoList = list;
            return result;
        }

        public List<IPRegionPairView> GetAllRegionStatus()
        {
            List<IPRegionPairView> result = new List<IPRegionPairView>();
            List<IPRegionPair> list = null;

            try
            {
                list = RedisHelper.GetIPList();

                foreach (IPRegionPair item in list)
                {
                    item.Status = RedisHelper.GetIPStatus(item.IP).ToString();
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            if (null != list)
            {
                List<IGrouping<string, IPRegionPair>> group = list.GroupBy(x => x.Region).ToList();

                foreach (IGrouping<string, IPRegionPair> item in group)
                {
                    if (item.Where(x => x.Status == "Impeded").Count() > 0)
                    {
                        result.Add(new IPRegionPairView() { Location = item.Key, Status = "Yellow" });
                    }
                    else if (item.Where(x => x.Status == "Invalid").Count() > 0)
                    {
                        result.Add(new IPRegionPairView() { Location = item.Key, Status = "Red" });
                    }
                    else if (item.Where(x => x.Status == "Unimpeded").Count() > 0)
                    {
                        result.Add(new IPRegionPairView() { Location = item.Key, Status = "Green" });
                    }
                    else if (item.Where(x => x.Status == "Unknow").Count() > 0)
                    {
                        result.Add(new IPRegionPairView() { Location = item.Key, Status = "Gray" });
                    }
                }
            }

            return result;
        }

        public List<IPRegionPairView> GetIPMonitorListStatus(IPRegionListCriteria criteria)
        {
            List<IPRegionPair> list = null;
            List<IPRegionPairView> result = new List<IPRegionPairView>();

            try
            {
                list = RedisHelper.GetIPList();

                foreach (IPRegionPair item in list)
                {
                    item.Status = RedisHelper.GetIPStatus(item.IP).ToString();
                }

                if (null != list)
                {
                    foreach (IPRegionPair item in list)
                    {
                        result.Add(new IPRegionPairView()
                        {
                            Location = item.Region + "-" + item.IP.Replace(".", string.Empty),
                            Status = item.Status
                        });
                    }
                }

                if (result.Count > 0)
                {
                    foreach(IPRegionPairView item in result)
                    {
                        switch(item.Status)
                        {
                            case "Impeded":
                                item.Status = "阻塞";
                                break;
                            case "Unknow":
                                item.Status = "未开始";
                                break;
                            case "Unimpeded":
                                item.Status = "畅通";
                                break;
                            case "Invalid":
                                item.Status = "断开";
                                break;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return result;
        }

        public bool IsExist(long sid, string ip)
        {
            bool result = false;

            try
            {
                result = manager.IsExist(sid, ip);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return result;
        }
    }
}
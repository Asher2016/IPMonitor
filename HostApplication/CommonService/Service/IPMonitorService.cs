using CommonContract.Model;
using CommonContract.Service;
using DataAccess.DAO;
using PlatForm.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CommonService.Service
{
    public class IPMonitorService : IIPMonitorService
    {
        public List<IPRegionPair> GetIPListForMonitor()
        {
            List<IPRegionPair> result = null;

            try
            {
                using (IPMonitorDAO dao = new IPMonitorDAO())
                {
                    result = dao.GetIpListForMonitor();
                }

                foreach (IPRegionPair item in result)
                {
                    item.Status = RedisHelper.GetIPStatus(item.IP).ToString();
                }

            }
            catch (Exception e)
            {
                LogHelper.Instance.Error(LogHelper.CommonService, "execute GetIpListForMonitor error " + e.Message);
                throw new FaultException("Internal Server Error");
            }

            return result;
        }

        public IPMonitorListModel GetIPRegionList(IPRegionListCriteria criteria)
        {
            IPMonitorListModel result = null;

            try
            {
                using (IPMonitorDAO dao = new IPMonitorDAO())
                {
                    result = dao.GetIPRegionList(criteria);
                }
            }
            catch (Exception e)
            {
                LogHelper.Instance.Error(LogHelper.CommonService, "execute GetIpListForMonitor error " + e.Message);
                throw new FaultException("Internal Server Error");
            }

            return result;
        }

        public void AddOrUpdateIPRegion(BrefIPRegionInfo brefIpRegionInfo)
        {
            try
            {
                using (IPMonitorDAO dao = new IPMonitorDAO())
                {
                    dao.AddOrUpdateIPRegion(brefIpRegionInfo);
                }
            }
            catch (Exception e)
            {
                LogHelper.Instance.Error(LogHelper.CommonService, "execute AddOrUpdateIPRegion error " + e.Message);
                throw new FaultException("Internal Server Error");
            }
        }

        public void DeleteIPRegion(long sid)
        {
            try
            {
                using (IPMonitorDAO dao = new IPMonitorDAO())
                {
                    dao.DeleteIPRegion(sid);
                }
            }
            catch (Exception e)
            {
                LogHelper.Instance.Error(LogHelper.CommonService, "execute AddOrUpdateIPRegion error " + e.Message);
                throw new FaultException("Internal Server Error");
            }
        }

        public BrefIPRegionInfo EditIPRegion(long sid)
        {
            BrefIPRegionInfo result = null;

            try
            {
                using (IPMonitorDAO dao = new IPMonitorDAO())
                {
                    result = dao.EditIPRegion(sid);
                }
            }
            catch (Exception e)
            {
                LogHelper.Instance.Error(LogHelper.CommonService, "execute AddOrUpdateIPRegion error " + e.Message);
                throw new FaultException("Internal Server Error");
            }

            return result;
        }

        public MonitorRecordListModel GetMonitorRecord(MonitorRecordCriteria criteria)
        {
            MonitorRecordListModel result = null;

            try
            {
                using (IPMonitorDAO dao = new IPMonitorDAO())
                {
                    result = dao.GetMonitorRecord(criteria);
                }
            }
            catch (Exception e)
            {
                LogHelper.Instance.Error(LogHelper.CommonService, "execute GetIpListForMonitor error " + e.Message);
                throw new FaultException("Internal Server Error");
            }

            return result;
        }

        public AlertInfoListModel GetAlertInfo(AlertInfoCriteria criteria)
        {
            AlertInfoListModel result = null;

            try
            {
                using (IPMonitorDAO dao = new IPMonitorDAO())
                {
                    result = dao.GetAlertInfo(criteria);
                }
            }
            catch (Exception e)
            {
                LogHelper.Instance.Error(LogHelper.CommonService, "execute GetIpListForMonitor error " + e.Message);
                throw new FaultException("Internal Server Error");
            }

            return result;
        }

        public List<IPRegionPair> GetAllIpListStatus()
        {
            List<IPRegionPair> result = null;

            try
            {
                using (IPMonitorDAO dao = new IPMonitorDAO())
                {
                    result = dao.GetAllIpListStatus();
                }

                foreach (IPRegionPair item in result)
                {
                    item.Status = RedisHelper.GetIPStatus(item.IP).ToString();
                }
            }
            catch (Exception e)
            {
                LogHelper.Instance.Error(LogHelper.CommonService, "execute GetAllIpListStatus error " + e.Message);
                throw new FaultException("Internal Server Error");
            }

            return result;
        }

        public List<IPRegionPair> GetIPRegionStatusPair(IPRegionListCriteria criteria)
        {
            IPMonitorListModel ipRegionList = null;
            List<IPRegionPair> result = new List<IPRegionPair>();

            try
            {
                ipRegionList = GetIPRegionList(criteria);

                if (null != ipRegionList)
                {
                    result.AddRange(ipRegionList.IPRegionList.Select(x => new IPRegionPair() { IP = x.IP, Region = x.Region }));
                }

                foreach(IPRegionPair item in result)
                {
                    item.Status = RedisHelper.GetIPStatus(item.IP).ToString();
                }
            }
            catch (Exception e)
            {
                LogHelper.Instance.Error(LogHelper.CommonService, "execute GetIPRegionStatusPair error " + e.Message);
                throw new FaultException("Internal Server Error");
            }

            return result;
        }


        public bool IsExist(long sid, string ip)
        {
            bool result = false;

            try
            {
                using (IPMonitorDAO dao = new IPMonitorDAO())
                {
                    result = dao.IsExist(sid, ip);
                }
            }
            catch (Exception e)
            {
                LogHelper.Instance.Error(LogHelper.CommonService, "execute IsExist error " + e.Message);
                throw new FaultException("Internal Server Error");
            }

            return result;
        }
    }
}

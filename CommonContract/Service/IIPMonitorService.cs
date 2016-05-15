using CommonContract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CommonContract.Service
{
    [ServiceContract]
    public interface IIPMonitorService
    {
        //[OperationContract]
        //[FaultContract(typeof(FaultException))]
        //List<IPRegionPair> GetIPListForMonitor(IPRegionPair criteria);

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        List<IPRegionPair> GetAllIpListStatus();

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        IPMonitorListModel GetIPRegionList(IPRegionListCriteria criteria);

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        List<IPRegionPair> GetIPRegionStatusPair(IPRegionListCriteria criteria);

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        void AddOrUpdateIPRegion(BrefIPRegionInfo brefIpRegionInfo);

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        void DeleteIPRegion(long sid);

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        BrefIPRegionInfo EditIPRegion(long sid);

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        MonitorRecordListModel GetMonitorRecord(MonitorRecordCriteria criteria);

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        AlertInfoListModel GetAlertInfo(AlertInfoCriteria criteria);

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        bool IsExist(long sid, string ip);
    }
}

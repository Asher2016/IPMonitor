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
    public interface IUserIPMapService
    {
        [OperationContract]
        [FaultContract(typeof(FaultException))]
        UserIPList Search(UserIPCriteria criteria);

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        void Delete(long sid);

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        UserIPInfo Edit(long sid);

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        void AddOrUpdate(UserIPInfo userIPInfo);

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        bool IsExist(long sid, string ipAddress);
    }
}

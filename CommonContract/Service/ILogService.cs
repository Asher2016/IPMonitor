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
    public interface ILogService
    {
        [OperationContract]
        [FaultContract(typeof(FaultException))]
        LogListContract SearchList(LogCriteria criteria);
    }
}

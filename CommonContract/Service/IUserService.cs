using CommonContract.Model;
using System.ServiceModel;

namespace CommonContract.Service
{
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        [FaultContract(typeof(FaultException))]
        BrefUserInfo CheckUserNameAndPassword(BrefUserInfo brefUserInfo);

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        void ChangePasswd(UserInfo userInfo);

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        bool CheckOldPasswd(UserInfo userInfo);
    }
}

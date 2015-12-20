using CommonContract.Model;
using DataAccess.DAO;
using PlatForm.Util;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommonService.ScheduleJob
{
    public class SendMessageJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            List<BrefSendMessageInfo> sendMessageInfo = new List<BrefSendMessageInfo>();
            List<long> finishSendList = new List<long>();
            try
            {
                using (AlertDAO dao = new AlertDAO())
                {
                    sendMessageInfo = dao.GetAlertInfo();
                }

                foreach(BrefSendMessageInfo item in sendMessageInfo)
                {
                    string message = String.Format("IP:{0}  第一次丢失时间: {1}  第二次丢失时间: {2}  恢复时间: {3}.",
                        item.IP,
                        item.FirstLostTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        item.SecondLostTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        item.RecoveryTime == DateTime.MinValue ? "未恢复" : item.RecoveryTime.ToString("yyyy-MM-dd HH:mm:ss"));

                    LogHelper.Instance.Info(LogHelper.CommonService, "Begin Send ");
                    string[] telephoneArray = item.Telephone.Split(',');
                    int resultCode = -99;

                    foreach(string telephoneItem in telephoneArray)
                    {
                        string tempTelephoneItem = telephoneItem.Trim();
                        resultCode = SendMessage.ExecuteProcessSendMessage(String.Format("{0} \"{1}\"", item.Telephone, message));
                    }

                    if (resultCode == 0)
                    {
                        finishSendList.Add(item.SID);
                    }
                }

                if (finishSendList.Count > 0)
                {
                    using (AlertDAO dao = new AlertDAO())
                    {
                        dao.FinishSendMessage(finishSendList);
                    }
                }
            }
            catch
            {
                LogHelper.Instance.Info(LogHelper.CommonService, "execute SendMessage error ");
            }
        }
    }
}

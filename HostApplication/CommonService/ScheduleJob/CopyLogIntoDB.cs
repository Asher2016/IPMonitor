using CommonConst;
using CommonService.LogInformation;
using CommonService.Manager;
using DataAccess.Model;
using PlatForm.Util;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommonService.ScheduleJob
{
    public class CopyLogIntoDB : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            string startupPath = (string)context.JobDetail.JobDataMap[ScheduleJobConst.StartupPath];
            string filePath = (string)context.JobDetail.JobDataMap[ScheduleJobConst.LogPath];
            SerializableDictionary<string, DateFormatInfo> dateFormatInfoDic =
                (SerializableDictionary<string, DateFormatInfo>)context.JobDetail.JobDataMap[ScheduleJobConst.DateFormatInfoDic];
            CutomerDataBase customDB = (CutomerDataBase)context.JobDetail.JobDataMap[ScheduleJobConst.CustomDB];
            string tableStruct = (string)context.JobDetail.JobDataMap[ScheduleJobConst.TableStruct];
            string targetFile = string.Empty;

            // Stop 3CSyslog Process
            try
            {
                Stop3CSyslogProcess(startupPath);
                LogHelper.Instance.Info(LogHelper.CommonService, string.Format("Stop 3CSyslog Process Success, Date {0}", DateTime.Now));
            }
            catch (Exception exception)
            {
                LogHelper.Instance.Info(LogHelper.CommonService, string.Format("Stop 3CSyslog Process Fail, Error Message {0}", exception));
            }

            try
            {
                targetFile = new TranslateFileFormat().ConvertFormatToCSV(filePath, dateFormatInfoDic);
            }
            catch (Exception exception)
            {
                LogHelper.Instance.Error(LogHelper.CommonService, string.Format("ConvertLogFormat Fail. Message {0}.", exception.Message));
            }

            try
            {
                LogFileManager manager = new LogFileManager();
                manager.RunCopy(customDB, tableStruct, targetFile);
            }
            catch (Exception exception)
            {
                LogHelper.Instance.Error(LogHelper.CommonService, string.Format("RunCopy Fail. Message {0}.", exception.Message));
            }

            try
            {
                DeleteOldFile(filePath);
            }
            catch (Exception exception)
            {
                LogHelper.Instance.Error(LogHelper.CommonService, string.Format("DeleteOldFile Fail. Message {0}.", exception.Message));
            }

            try
            {
                Start3CSyslogProcess(startupPath);
                LogHelper.Instance.Info(LogHelper.CommonService, string.Format("Start 3CSyslog Process Success, Date {0}", DateTime.Now));
            }
            catch(Exception exception)
            {
                LogHelper.Instance.Info(LogHelper.CommonService, string.Format("Start 3CSyslog Process Fail, Error Message {0}", exception));
            }
        }

        public void Stop3CSyslogProcess(string startupPath)
        {
            Process[] processes = Process.GetProcessesByName("3CSyslog");
            try
            {
                foreach (Process p in processes)
                {
                    if (System.IO.Path.Combine(startupPath.Replace("\\\\", "\\"), "3CSyslog.EXE").Equals(p.MainModule.FileName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        p.Kill();
                        p.Close();
                    }
                }
            }
            catch
            {

            }
        }

        public void Start3CSyslogProcess(string startupPath)
        {
            Process process = new Process();
            process.StartInfo.FileName = System.IO.Path.Combine(startupPath, "3CSyslog.EXE");
            process.StartInfo.UseShellExecute = true;
            process.Start();
           Thread.Sleep(3000);
        }

        public void DeleteOldFile(string filePath)
        {
            string[] files = Directory.GetFiles(filePath, "*.log", SearchOption.TopDirectoryOnly);

            foreach(string path in files)
            {
                try
                {
                    File.Delete(path);
                    LogHelper.Instance.Info(LogHelper.CommonService, string.Format("Delete file {0} success ~", path));
                }
                catch
                {
                    LogHelper.Instance.Error(LogHelper.CommonService, string.Format("Delete file {0} fail ~", path));
                }
            }
        }
    }
}

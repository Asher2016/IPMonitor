using CommonConst;
using DataAccess.Model;
using PlatForm.Util;
using System;
using System.Diagnostics;
using System.Threading;

namespace CommonService.Manager
{
    public class LogFileManager
    {
        public void RunCopy(CutomerDataBase customDB, string source, string filePath)
        {
            try
            {
                ImportCSVFile(customDB, filePath, source);
            }
            catch(Exception exception)
            {
                LogHelper.Instance.Error(LogHelper.CommonService, exception.Message);
            }
        }

        public void ImportCSVFile(CutomerDataBase suite, string filePath, string source)
        {
            string processArguments = string.Format(
                LogInfoConst.ProcessCopyFromArguments,
                suite.ServerName,
                suite.Database,
                source,
                filePath);

            Console.WriteLine(processArguments);
            int result = ExecuteProcess(processArguments, suite);
            Console.WriteLine(result);
        }

        public int ExecuteProcess(string arguments, CutomerDataBase suite)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.Arguments = arguments;
            info.FileName = Resolver.ResolvePath(LogInfoConst.PSQL);
            info.EnvironmentVariables[LogInfoConst.PGUSER] = suite.Username;
            info.EnvironmentVariables[LogInfoConst.PGPASSWORD] = suite.Password;
            info.EnvironmentVariables[LogInfoConst.PGCLIENTENCODING] = LogInfoConst.UTF8;

            info.CreateNoWindow = true;
            info.UseShellExecute = false;
            info.RedirectStandardInput = true;
            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;

            var process = System.Diagnostics.Process.Start(info);
            while (true)
            {
                // Wait for the process exits. The WaitForExit() method doesn't work
                if (process.HasExited)
                {
                    break;
                }

                Thread.Sleep(100);
            }

            return process.ExitCode;
        }
    }
}

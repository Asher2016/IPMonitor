using CommonConst;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace PlatForm.Util
{
    public static class SendMessage
    {
        public static int ExecuteProcessSendMessage(string arguments)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.Arguments = arguments;
            info.FileName = Resolver.ResolvePath(LogInfoConst.SMSConsole);

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

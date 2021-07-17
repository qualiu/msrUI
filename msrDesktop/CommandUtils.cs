using System;
using System.Diagnostics;
using System.Text;

namespace msrDesktop
{
    public static class CommandUtils
    {
        public static (string stdout, string error, int exitCode) RunCommand(
            string exe,
            string args,
            DataReceivedEventHandler outputHandler = null,
            DataReceivedEventHandler errorHander = null,
            EventHandler exitHandler = null)
        {
            var startInfo = new ProcessStartInfo()
            {
                FileName = exe,
                Arguments = args,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WindowStyle = ProcessWindowStyle.Hidden,
            };

            var process = Process.Start(startInfo);

            var outputBuffer = new StringBuilder();
            var errorBuffer = new StringBuilder();
            process.OutputDataReceived += (sender, e) =>
            {
                if (e.Data == null)
                {
                    return;
                }

                if (outputHandler != null)
                {
                    outputHandler(sender, e);
                }
                else
                {
                    outputBuffer.AppendLine(e.Data);
                }
            };

            process.ErrorDataReceived += (sender, e) =>
            {
                if (e.Data == null)
                {
                    return;
                }

                if (errorHander != null)
                {
                    errorHander(sender, e);
                }
                else
                {
                    errorBuffer.AppendLine(e.Data);
                }
            };

            if (exitHandler != null)
            {
                process.Exited += exitHandler;
            }

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
            return (outputBuffer.ToString(), errorBuffer.ToString(), process.ExitCode);
        }
    }
}

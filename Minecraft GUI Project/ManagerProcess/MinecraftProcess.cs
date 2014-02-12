using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Minecraft_GUI_Project.ManagerProcess
{
    public class MinecraftProcess
    {
        public static Process Process;
        private string _diretory;
        public String Status;

        public void StartProcess()
        {
            _diretory = Properties.Settings.Default.WorkingDiretory;
            if (String.IsNullOrWhiteSpace(_diretory))
                _diretory = "\\";

            var processInfo = new ProcessStartInfo("java.exe", "-jar craftbukkit.jar")
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                RedirectStandardError = true,
                StandardErrorEncoding = Encoding.UTF8,
                StandardOutputEncoding = Encoding.UTF8,
                WorkingDirectory = _diretory
            };

            Process = new Process { StartInfo = processInfo };
            Process.Start();
        }

        public static void SendCommand(String command)
        {
            byte[] buffer = Encoding.GetEncoding("utf-8").GetBytes(command + "\r");
            Process.StandardInput.BaseStream.Write(buffer, 0, buffer.Length);
            Process.StandardInput.Flush();
        }

    }
}

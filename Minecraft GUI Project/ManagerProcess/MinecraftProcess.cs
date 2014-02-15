using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Minecraft_GUI_Project.ManagerProcess
{
    public class MinecraftProcess
    {
        private String _javadir;
        public static Process Process;
        private string _diretory;
        public String Status;

        public void StartProcess()
        {
            _diretory = Properties.Settings.Default.WorkingDiretory;
            if (String.IsNullOrWhiteSpace(_diretory))
                _diretory = "\\";

            string javadir;
            LocateJava();
            if (!_javadir.Equals(String.Empty)) {
                javadir = this._javadir + "java.exe";
            } else {
                javadir = "java.exe";
            }

            var processInfo = new ProcessStartInfo(javadir, Properties.Settings.Default.JavaParam + " -jar craftbukkit.jar " + Properties.Settings.Default.JarParam)
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

        private void LocateJava()
        {
            String path = Environment.GetEnvironmentVariable("path");
            if (path == null) return;
            String[] folders = path.Split(';');
            foreach (String folder in folders)
            {
                if (File.Exists(folder + "java.exe"))
                {
                    _javadir = folder;
                    return;
                }
                if (!File.Exists(folder + "\\java.exe")) continue;

                _javadir = folder + "\\";
                return;
            }
        }

        public static void SendCommand(String command)
        {
            byte[] buffer = Encoding.GetEncoding("utf-8").GetBytes(command + "\r");
            Process.StandardInput.BaseStream.Write(buffer, 0, buffer.Length);
            Process.StandardInput.Flush();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Minecraft_GUI_Project.ManagerProcess;
using Minecraft_GUI_Project.Pages;

namespace Minecraft_GUI_Project
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            string lang = Minecraft_GUI_Project.Properties.Settings.Default.Lang;
            if (lang != null || !String.IsNullOrWhiteSpace(lang))
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(lang);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
            }
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (MinecraftProcess.Process != null)
            {
                MinecraftProcess.SendCommand("stop");
                MinecraftProcess.Process.WaitForExit();
            }
            base.OnExit(e);
        }
    }
}

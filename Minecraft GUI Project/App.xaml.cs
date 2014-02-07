using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Minecraft_GUI_Project.Pages;

namespace Minecraft_GUI_Project
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnExit(ExitEventArgs e)
        {
            if (Home.Proc != null)
            {
                Home.SendCommand("stop");
                Home.Proc.WaitForExit();
            }
            base.OnExit(e);
        }
    }
}

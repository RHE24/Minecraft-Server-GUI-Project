using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Minecraft_GUI_Project.ManagerProcess;

namespace Minecraft_GUI_Project.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        public MinecraftProcess MinecraftProcess;
        public Home()
        {
            InitializeComponent();
            MinecraftProcess = new MinecraftProcess();
        }

        private void SendCommand(object sender, RoutedEventArgs e)
        {
           MinecraftProcess.SendCommand(InputCommand.Text);
        }

        private void Start(object sender, RoutedEventArgs e)
        {
            MinecraftProcess.StartProcess();

            MinecraftProcess.Process.ErrorDataReceived += ProcessOnErrorDataReceived;
            MinecraftProcess.Process.BeginErrorReadLine();
            MinecraftProcess.Process.OutputDataReceived += OnDataReceived;
            MinecraftProcess.Process.BeginOutputReadLine();

            DispatcherTimer timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 2) };
            timer.Tick += OnTimerOnTick;
            timer.Start();
        }

        private void ProcessOnErrorDataReceived(object sender, DataReceivedEventArgs args)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(() => { Console.Text = Console.Text + "\n " + args.Data; }));
        }

        public void Stop(object sender, RoutedEventArgs e)
        {
            MinecraftProcess.SendCommand("stop");
        }

        private void OnTimerOnTick(object sender, EventArgs e)
        {
            if (ScrollView.VerticalOffset.Equals(ScrollView.ScrollableHeight))
            {
                ScrollView.ScrollToEnd();
            }
        }

        private void OnDataReceived(object sender, DataReceivedEventArgs args)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(() => { Console.Text = Console.Text + "\n " + args.Data; }));
        }

    }
}

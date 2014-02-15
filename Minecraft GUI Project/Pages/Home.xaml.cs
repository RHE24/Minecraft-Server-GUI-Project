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

        public void Download(object sender, RoutedEventArgs e)
        {
            
        }

        private void Start(object sender, RoutedEventArgs e)
        {
            MinecraftProcess.StartProcess();

            MinecraftProcess.Process.ErrorDataReceived += OnErrorReceived;
            MinecraftProcess.Process.BeginErrorReadLine();
            MinecraftProcess.Process.OutputDataReceived += OnDataReceived;
            MinecraftProcess.Process.BeginOutputReadLine();

            DispatcherTimer timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 2) };
            timer.Tick += OnTimerOnTick;
            timer.Start();
        }

        private void OnDataReceived(object sender, DataReceivedEventArgs args)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(() => { Console.Text = Console.Text + "\n " + args.Data; }));
        }

        private void OnErrorReceived(object sender, DataReceivedEventArgs args)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(() => { Console.Text = Console.Text + "\n " + args.Data; }));
        }

        public void Stop(object sender, RoutedEventArgs e)
        {
            MinecraftProcess.SendCommand("stop");
        }

        private void OnTimerOnTick(object sender, EventArgs e)
        {
            if (MinecraftProcess.Process.HasExited)
            {
                MemoryValue.Text = "0 MB";
                CpuValue.Text = "0 %";
                return;
            }
                

            if (ScrollView.VerticalOffset.Equals(ScrollView.ScrollableHeight))
            {
                ScrollView.ScrollToEnd();
            }

            SetCpu();
            SetMemory();
        }

        private void SetCpu()
        {
            new Thread(() =>
            {
                if(MinecraftProcess.Process.HasExited) return;

                PerformanceCounter performanceCounter = new PerformanceCounter
                {
                    CategoryName = "Process",
                    CounterName = "% Processor Time",
                    InstanceName = MinecraftProcess.Process.ProcessName
                };
                performanceCounter.NextValue();
                Thread.Sleep(1000);

                if (MinecraftProcess.Process.HasExited) return;

                Dispatcher.BeginInvoke(DispatcherPriority.Background, new ThreadStart(() => CpuValue.Text = (performanceCounter.NextValue() / Environment.ProcessorCount).ToString("N2") + " %"));
            }).Start();
        }

        private void SetMemory()
        {
            PerformanceCounter performanceCounter = new PerformanceCounter
            {
                CategoryName = "Process",
                CounterName = "Working Set",
                InstanceName = MinecraftProcess.Process.ProcessName
            };
            string text = ((uint)performanceCounter.NextValue()/1024/1000).ToString("N0") + " MB";
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(() => MemoryValue.Text = text));
        }

    }
}

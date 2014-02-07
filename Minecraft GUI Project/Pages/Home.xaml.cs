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

namespace Minecraft_GUI_Project.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        public static Process Proc;
        public Home()
        {
            InitializeComponent();
        }

        public void IniciarProcesso()
        {
            var processInfo = new ProcessStartInfo("java.exe", "-jar craftbukkit.jar")
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                RedirectStandardError = true,
                StandardErrorEncoding = Encoding.UTF8,
                StandardOutputEncoding = Encoding.UTF8,
                WorkingDirectory = "C:\\Users\\Daniel\\Desktop\\Servidor Teste"
            };


            if ((Proc = Process.Start(processInfo)) == null)
            {
                throw new InvalidOperationException("??");
            }

            Proc.OutputDataReceived += OnDataReceived;
            Proc.BeginOutputReadLine();

            DispatcherTimer timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 2) };
            timer.Tick += OnTimerOnTick;
            timer.Start();
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
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(() => { Console.Text = Console.Text + "\n" + args.Data; }));
        }

        public static void SendCommand(String command)
        {
            byte[] buffer = Encoding.GetEncoding("utf-8").GetBytes(command + "\r");
            Proc.StandardInput.BaseStream.Write(buffer, 0, buffer.Length);
            Proc.StandardInput.Flush();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SendCommand(InputCommand.Text);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            IniciarProcesso();
        }
    }
}

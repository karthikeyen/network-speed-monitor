// https://github.com/murrayju/CreateProcessAsUser

using System;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Timers;
using System.Windows;

namespace NetworkMon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Timer _timer = new Timer();
        private SysTray sysTray = new SysTray("");
        
        static IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
        static IPGlobalStatistics ipstat = properties.GetIPv4GlobalStatistics();

        static NetworkInterface[] fNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

        EventLog eventLog = new EventLog();

        // https://www.codeproject.com/Articles/36468/WPF-NotifyIcon-2
        public MainWindow()
        {
            InitializeComponent();

            this.Initialized += MainWindow_Initialized;
            this.Loaded += MainWindow_Loaded;
            this.Closed += MainWindow_Closed;

            this.Left = System.Windows.SystemParameters.PrimaryScreenWidth - 200;
            this.Top = System.Windows.SystemParameters.PrimaryScreenHeight - 100;

            this._timer.Elapsed += _timer_Elapsed;
            this._timer.Start();

            if (!EventLog.SourceExists("NetMonSource"))
            {
                EventLog.CreateEventSource("NetMonSource", "Application");
            }

            eventLog.Source = "NetMonSource";
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            this._timer.Stop();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            eventLog.WriteEntry("Process started", EventLogEntryType.Information);
            this.sysTray.SetDefaultIcons();
        }

        private void MainWindow_Initialized(object sender, EventArgs e)
        {
            
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (this.sysTray.IsRunning)
            {
                getConnectionInfo();
            }
            else
            {
                this._timer.Stop();
            }
        }

        private void getConnectionInfo()
        {
            try
            {
                string myHost = System.Net.Dns.GetHostName();
                System.Net.IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(myHost);
                IPAddress[] addr = ipEntry.AddressList;
                // NetworkChange.NetworkAvailabilityChanged += new NetworkAvailabilityChangedEventHandler(NetworkChange_NetworkAvailabilityChanged);
                var _isNetworkOnline = NetworkInterface.GetIsNetworkAvailable();

                if (addr.Length > 0)
                {
                    NetworkInterface adapter = fNetworkInterfaces[2];
                    long received = adapter.GetIPv4Statistics().BytesReceived;

                    int speed = Speed(received);

                    prevValue = received;
                    prevTime = DateTime.Now;

                    string quicktext = "";
                    if (speed == 0)
                    {
                        quicktext = $"--";
                        this.sysTray.ClearTrayIcon();
                    }
                    else if (speed > 0 && speed < 1024)
                    {
                        quicktext = $"{speed} KB/s";
                        this.sysTray.ShowTrayDonwloadIcon();
                    }
                    else if (speed > 1024)
                    {
                        quicktext = $"{speed / 1024} MB/s";
                        this.sysTray.ShowTrayDonwloadIcon();
                    }

                    this.Dispatcher.Invoke(() =>
                    {
                        this.lbl.Text = speed == 0 ? "" : quicktext;
                    });
                }
            }
            catch (Exception ex)
            {
                eventLog.WriteEntry(ex.ToString(), EventLogEntryType.Error);
                this._timer.Stop();
            }
        }

        static long prevValue;
        static int Speed(long received)
        {
            long recievedBytes = received - prevValue;
            return (int)recievedBytes / 1024;
        }
    }
}

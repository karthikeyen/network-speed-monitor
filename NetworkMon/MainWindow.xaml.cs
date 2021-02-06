using NetworkMon.UI;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Timers;
using System.Windows;
using System.Windows.Media;

namespace NetworkMon
{
    public partial class MainWindow : Window
    {
        public bool IsRunning = true;
        private Timer _timer = new Timer();
        private THEME Theme;
        private EventLog eventLog = new EventLog();
        static NetworkInterface[] fNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
            this.Closed += MainWindow_Closed;

            this.Left = SystemParameters.PrimaryScreenWidth - 200;
            this.Top = SystemParameters.PrimaryScreenHeight - 100;

            this._timer.Elapsed += _timer_Elapsed;
            this._timer.Start();

            if (!EventLog.SourceExists("NetMonSource"))
            {
                EventLog.CreateEventSource("NetMonSource", "Application");
            }

            eventLog.Source = "NetMonSource";
            SetColors();

            TrayIconManager.SetupTrayIcon();
        }

        private void SetColors()
        {
            Theme = ThemeHelper.GetTheme();
            if (Theme == THEME.DARK)
            {
                this.lbl.Foreground = new System.Windows.Media.SolidColorBrush(Colors.White);
            }
            else
            {
                this.lbl.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#3B3B3B"));
            }
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            this._timer.Stop();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            eventLog.WriteEntry("Process started", EventLogEntryType.Information);
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (this.IsRunning)
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
                    Debug.WriteLine(speed);

                    prevValue = received;

                    string quicktext = "";
                    if (speed == 0)
                    {
                        quicktext = $"--";
                        // this.sysTray.ClearTrayIcon();
                    }
                    else if (speed > 0 && speed < 1024)
                    {
                        quicktext = $"{speed} KB/s";
                        // this.sysTray.ShowTrayDonwloadIcon();
                    }
                    else if (speed > 1024)
                    {
                        quicktext = $"{speed / 1024} MB/s";
                        // this.sysTray.ShowTrayDonwloadIcon();
                    }

                    this.Dispatcher.Invoke(() =>
                    {
                        // App.Current.FindResource
                        this.lbl.Text = speed == 0 ? "--" : quicktext;
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

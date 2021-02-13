using NetworkMon.Helpers;
using NetworkMon.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Timers;
using System.Windows;

namespace NetworkMon
{
    public partial class MainWindow : Window
    {
        public bool IsRunning = true;
        private Timer _timer = new Timer();
        private EventLog eventLog = new EventLog();

        private int count = 3;
        private List<double> buffer = new List<double>();
        private double _speed;

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
            this.Closed += MainWindow_Closed;

            this.Left = SystemParameters.PrimaryScreenWidth - 250;
            this.Top = SystemParameters.PrimaryScreenHeight - 100;

            this._timer.Elapsed += _timer_Elapsed;
            this._timer.Start();

            if (!EventLog.SourceExists("NetMonSource"))
            {
                EventLog.CreateEventSource("NetMonSource", "Application");
            }

            eventLog.Source = "NetMonSource";

            TrayIconManager.SetupTrayIcon();
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            this._timer.Stop();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            eventLog.WriteEntry("Process started", EventLogEntryType.Information);

            StartupHelper.SetRunAtStartupEnabled(true);
        }

        NetworkInterface GetConnectedNetworkInterface()
        {
            List<NetworkInterface> Interfaces = new List<NetworkInterface>();
            var all = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var ni in all)
            {
                if (ni.OperationalStatus == OperationalStatus.Up && ni.NetworkInterfaceType != NetworkInterfaceType.Tunnel && ni.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                {
                    Interfaces.Add(ni);
                }
            }

            if (Interfaces.Count > 0)
            {
                return Interfaces[0];
            }
            else
            {
                return null;
            }
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (this.IsRunning && NetworkInterface.GetIsNetworkAvailable())
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
                NetworkInterface adapter = GetConnectedNetworkInterface();
                if (adapter == null) return;

                long received = adapter.GetIPv4Statistics().BytesReceived;
                string log = string.Empty;

                double speed = Speed(received);
                prevValue = received;

                if (buffer.Count < count)
                {
                    buffer.Add(speed);
                }

                if (buffer.Count == count)
                {
                    _speed = buffer.Average(item => item);

                    string quicktext = String.Empty;
                    if (speed == 0)
                    {
                        quicktext = $"-";
                    }
                    else if (speed > 1 && speed < 1024)
                    {
                        quicktext = $"{speed.ToString("#")} KB/s";
                    }
                    else if (speed > 1024)
                    {
                        quicktext = $"{(speed / 100f).ToString("#.#")} MB/s";
                    }

                    this.Dispatcher.Invoke(() =>
                    {
                        this.lbl.Text = speed == 0 ? String.Empty : quicktext;
                    });

                    buffer.Clear();
                }
            }
            catch (Exception ex)
            {
                eventLog.WriteEntry(ex.ToString(), EventLogEntryType.Error);
                this._timer.Stop();
            }
        }

        static long prevValue;
        static double Speed(long received)
        {
            long recievedBytes = received - prevValue;
            if (recievedBytes > 0)
            {
                Debug.WriteLine($"{recievedBytes}={(recievedBytes / 1024f).ToString("#.##")}");
            }
            return (recievedBytes / 1024f);
        }
    }
}

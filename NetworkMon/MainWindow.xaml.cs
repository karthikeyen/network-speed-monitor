using Core;
using NetworkMon.Helpers;
using NetworkMon.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows;

namespace NetworkMon
{
    public partial class MainWindow : Window
    {
        public bool IsRunning = true;
        private EventLog eventLog = new EventLog();

        private NetworkInterface myNetworkAdapter;
        private NetworkSpeed myDownloadSpeed;
        private NetworkSpeed myUploadSpeed;

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
            this.Closed += MainWindow_Closed;

            this.Left = SystemParameters.PrimaryScreenWidth - 250;
            this.Top = SystemParameters.PrimaryScreenHeight - 100;

            if (!EventLog.SourceExists("NetMonSource"))
            {
                EventLog.CreateEventSource("NetMonSource", "Application");
            }

            eventLog.Source = "NetMonSource";

            myDownloadSpeed = new NetworkSpeed();
            myUploadSpeed = new NetworkSpeed();

            TrayIconManager.SetupTrayIcon();
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            this.IsRunning = false;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            eventLog.WriteEntry("Process started", EventLogEntryType.Information);

            StartupHelper.SetRunAtStartupEnabled(true);

            new Thread(startTick).Start();
        }

        void startTick()
        {
            while (this.IsRunning)
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    getConnectionInfo();
                }
            }
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
                var interface_ = Interfaces.Find(x => !x.Description.Contains("Hyper-V"));
                return interface_;
            }
            else
            {
                return null;
            }
        }

        private void getConnectionInfo()
        {
            try
            {
                if (myNetworkAdapter == null)
                {
                    myNetworkAdapter = GetConnectedNetworkInterface();
                }

                long received = myNetworkAdapter.GetIPv4Statistics().BytesReceived;
                long sent = myNetworkAdapter.GetIPv4Statistics().BytesSent;

                var receivedSpeed = myDownloadSpeed.GetSpeed(received);
                var sentSpeed = myUploadSpeed.GetSpeed(sent);

                this.Dispatcher.Invoke(() =>
                {
                    this.lbl.Text = receivedSpeed;
                    this.lblUp.Text = sentSpeed;
                });
            }
            catch (Exception ex)
            {
                eventLog.WriteEntry(ex.ToString(), EventLogEntryType.Error);
            }
        }
    }
}

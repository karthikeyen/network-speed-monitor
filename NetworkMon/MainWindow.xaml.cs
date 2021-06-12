using NetworkMon.Core;
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
        private readonly EventLog eventLog = new EventLog();

        private NetworkInterface myNetworkAdapter;
        private readonly NetworkSpeed myDownloadSpeed;
        private readonly NetworkSpeed myUploadSpeed;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
            Closed += MainWindow_Closed;

            Left = SystemParameters.PrimaryScreenWidth - 250;
            Top = SystemParameters.PrimaryScreenHeight - 100;

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
            IsRunning = false;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            eventLog.WriteEntry("Process started", EventLogEntryType.Information);

            StartupHelper.SetRunAtStartupEnabled(true);

            new Thread(StartTick).Start();
        }

        private void StartTick()
        {
            while (IsRunning)
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    GetConnectionInfo();
                }
            }
        }

        private NetworkInterface GetConnectedNetworkInterface()
        {
            List<NetworkInterface> Interfaces = new List<NetworkInterface>();
            NetworkInterface[] all = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface ni in all)
            {
                if (ni.OperationalStatus == OperationalStatus.Up && ni.NetworkInterfaceType != NetworkInterfaceType.Tunnel && ni.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                {
                    Interfaces.Add(ni);
                }
            }

            if (Interfaces.Count > 0)
            {
                NetworkInterface interface_ = Interfaces.Find(x => !x.Description.Contains("Hyper-V"));
                return interface_;
            }
            else
            {
                return null;
            }
        }

        private void GetConnectionInfo()
        {
            try
            {
                if (myNetworkAdapter == null)
                {
                    myNetworkAdapter = GetConnectedNetworkInterface();
                }

                long received = myNetworkAdapter.GetIPv4Statistics().BytesReceived;
                long sent = myNetworkAdapter.GetIPv4Statistics().BytesSent;

                string receivedSpeed = myDownloadSpeed.GetSpeed(received);
                string sentSpeed = myUploadSpeed.GetSpeed(sent);

                Dispatcher.Invoke(() =>
                {
                    lbl.Text = receivedSpeed;
                    lblUp.Text = sentSpeed;
                });
            }
            catch (Exception ex)
            {
                eventLog.WriteEntry(ex.ToString(), EventLogEntryType.Error);
            }
        }
    }
}

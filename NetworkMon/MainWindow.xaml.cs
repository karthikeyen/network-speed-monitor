// https://github.com/murrayju/CreateProcessAsUser

using System;
using System.Diagnostics;
using System.Drawing;
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

        private SysTray sysTray = new SysTray("", new Icon(SystemIcons.Exclamation, 40, 40));

        private static bool _isNetworkOnline;
        static IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
        static IPGlobalStatistics ipstat = properties.GetIPv4GlobalStatistics();

        static Decimal start_r_packets;
        static Decimal end_r_packets;
        static NetworkInterface[] fNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
        static long start_received_bytes;
        static long start_sent_bytes;
        static long end_received_bytes;
        static long end_sent_bytes;

        EventLog eventLog = new EventLog();

        // https://www.codeproject.com/Articles/36468/WPF-NotifyIcon-2
        public MainWindow()
        {
            InitializeComponent();

            this.Initialized += MainWindow_Initialized;

            // eventLog.WriteEntry("Process started", EventLogEntryType.Information);

            this.Left = System.Windows.SystemParameters.PrimaryScreenWidth - 200;
            this.Top = System.Windows.SystemParameters.PrimaryScreenHeight - 100;

            this._timer.Elapsed += _timer_Elapsed;
            this._timer.Start();

            // Debugger.Launch();
            // Check if the event source exists. If not create it.
            if (!System.Diagnostics.EventLog.SourceExists("NetMonSource"))
            {
                System.Diagnostics.EventLog.CreateEventSource("NetMonSource", "Application");
            }

            eventLog.Source = "NetMonSource";
        }

        private void MainWindow_Initialized(object sender, EventArgs e)
        {
            eventLog.WriteEntry("Process started", EventLogEntryType.Information);
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            getConnectionInfo();
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
                    start_r_packets += Convert.ToDecimal(ipstat.ReceivedPackets);
                    NetworkInterface adapter = fNetworkInterfaces[2];

                    long received = adapter.GetIPv4Statistics().BytesReceived;
                    start_received_bytes += received;

                    start_sent_bytes = adapter.GetIPv4Statistics().BytesSent;
                    start_sent_bytes = (start_sent_bytes / 1048576 * 100000) / 100000;

                    int speed = Speed(received);
                    Console.WriteLine(speed);
                    Trace.WriteLine(speed);

                    prevValue = received;
                    prevTime = DateTime.Now;

                    string quicktext = "";
                    if (speed < 1024)
                    {
                        quicktext = $"{speed} KB/s";
                        this.sysTray.RenderMultiLineText(speed.ToString(), "KB", Color.White);
                    }
                    else if (speed > 1024)
                    {
                        quicktext = $"{speed / 1024} MB/s";
                        this.sysTray.RenderMultiLineText($"{speed / 1024}", "MB", Color.White);
                    }

                    this.Dispatcher.Invoke(() =>
                    {
                        // eventLog.WriteEntry("getConnectionInfo() :: "+ quicktext, EventLogEntryType.Information);
                        this.lbl.Text = quicktext;
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
        static DateTime prevTime;
        static int Speed(long received)
        {
            long recievedBytes = received - prevValue;
            return (int)recievedBytes / 1024;
        }
    }
}

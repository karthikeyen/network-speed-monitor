using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Timers;

namespace NetworkMonWinService
{
    public partial class WatchDogService : ServiceBase
    {
        Timer timer;
        public WatchDogService()
        {
            this.ServiceName = "KKB_net_mon";
            this.CanStop = true;

            InitializeComponent();

            timer = new Timer();
            timer.Interval = 5000;
            timer.Elapsed += Timer_Elapsed;

            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CheckRunningStatus();
        }

        protected override void OnStart(string[] args)
        {
            CheckRunningStatus();
        }

        private void CheckRunningStatus()
        {
            var path = "C:\\Services\\netcoreapp3.1\\NetworkMon.exe";
            var process_ = Process.GetProcessesByName("NetworkMon");
            if (process_.Length == 0)
            {
                try
                {
                    if (File.Exists(path))
                    {
                        EventLog.WriteEntry("NetworkMon.exe exists", EventLogEntryType.Information);
                        // Process.Start(path);

                        Process process = new Process()
                        {
                            
                        };

                        ProcessExtensions.StartProcessAsCurrentUser(path);

                        var StartInfo = new ProcessStartInfo(path)
                        {
                            WindowStyle = ProcessWindowStyle.Normal,
                            WorkingDirectory = Path.GetDirectoryName(path),
                            UseShellExecute = true,
                            Verb = "runas",
                        };
                        StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                        process.StartInfo = StartInfo;

                        // process.Start();
                        EventLog.WriteEntry("NetworkMon.exe started", EventLogEntryType.Information);
                    }
                    else
                    {
                        EventLog.WriteEntry("NetworkMon.exe does not exists", EventLogEntryType.Warning);
                    }
                }
                catch (System.Exception ex)
                {
                    EventLog.WriteEntry(ex.ToString(), EventLogEntryType.Error);
                }
            }
        }

        protected override void OnStop()
        {
            var process = Process.GetProcessesByName("NetworkMon");
            if (process.Length > 0)
            {
                foreach (var proces in process)
                {
                    proces.Kill();
                }
            }
        }
    }
}

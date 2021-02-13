using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.ApplicationModel;

namespace NetworkMon.Helpers
{
    internal class StartupHelper
    {
        private const string StartupId = "NetworkStatMonitorStartupId";

        public static async Task<bool> GetRunAtStartupEnabled()
        {
            try
            {
                StartupTask startupTask = await StartupTask.GetAsync(StartupId);

                return startupTask.State == StartupTaskState.Enabled;
            }
            catch { return true; }
        }

        public static async void SetRunAtStartupEnabled(bool value)
        {
            try
            {
                StartupTask startupTask = await StartupTask.GetAsync(StartupId);

                if (value)
                {
                    await startupTask.RequestEnableAsync();
                }
                else
                {
                    startupTask.Disable();
                }
            }
            catch (Exception)
            {
                RegisterInStartup(true);
            }
        }

        private static void RegisterInStartup(bool isChecked)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey
                    ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            var exe = Application.StartupPath + "NetworkMon.exe";
            if (isChecked)
            {
                registryKey.SetValue("Network Status Monitor", exe);
            }
            else
            {
                registryKey.DeleteValue("Network Status Monitor");
            }
        }
    }
}

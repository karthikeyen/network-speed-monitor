using Microsoft.Toolkit.Mvvm.ComponentModel;
using NetworkMon.Helpers;
using NetworkMon.UI;

namespace NetworkMon.Core
{
    public class Handler : ObservableObject
    {
        public UIManager UIManager { get; set; }
        public static Handler Instance { get; set; }

        private bool runAtStartup;
        public bool RunAtStartup
        {
            get => runAtStartup;
            set
            {
                if (SetProperty(ref runAtStartup, value))
                {
                    StartupHelper.RegisterInStartup(runAtStartup);
                }
            }
        }

        public void Init()
        {
            UIManager = new UIManager();

            RunAtStartup = !string.IsNullOrEmpty(StartupHelper.GetRegisterInStartup) ? true : false;
        }
    }
}

using Microsoft.Toolkit.Mvvm.ComponentModel;
using NetworkMon.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkMon.Core
{
    public class Handler : ObservableObject
    {
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
    }
}

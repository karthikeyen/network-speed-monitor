using Microsoft.Toolkit.Mvvm.Input;

namespace NetworkMon.Utilities
{
    public static class CommonCommands
    {
        public static RelayCommand SettingAppCommand { get; } =
        new RelayCommand(() =>
        {
            Settings a = Settings.Instance;
            a.Show();
        });

        public static RelayCommand RestartAppCommand { get; } =
        new RelayCommand(() =>
        {
            System.Windows.Application.Current.Shutdown();
        });

        public static RelayCommand OpenSettingsWindowCommand { get; } =
        new RelayCommand(() =>
        {

        });
    }
}

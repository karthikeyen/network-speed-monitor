using Microsoft.Win32;

namespace NetworkMon
{
    public enum THEME
    {
        DARK, LIGHT
    }

    public static class ThemeHelper
    {
        public static THEME GetTheme()
        {
            string RegistryKey = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize";
            int theme = (int)Registry.GetValue(RegistryKey, "AppsUseLightTheme", string.Empty);
            if (theme == 0)
            {
                return THEME.DARK;
            }
            else
            {
                return THEME.LIGHT;
            }
        }
    }
}

using Hardcodet.Wpf.TaskbarNotification;
using ModernWpf.Controls;
using NetworkMon.Utilities;
using System;
using System.Drawing;
using System.Windows.Controls;

namespace NetworkMon.UI
{
    internal class TrayIconManager
    {
        #region Properties

        public static TaskbarIcon TaskbarIcon { get; private set; }

        public static ContextMenu TaskbarIconContextMenu { get; private set; }

        public static ToolTip TaskbarIconToolTip { get; private set; }

        #endregion

        public static void SetupTrayIcon()
        {
            var settingsItem = new MenuItem()
            {
                Header = "Settings",
                ToolTip = "Settings for Application",
                Icon = new FontIcon() { Glyph = CommonGlyphs.Settings },
                Command = CommonCommands.SettingAppCommand
            };

            var exitItem = new MenuItem()
            {
                Header = "Exit",
                ToolTip = "Exit Live Status",
                Icon = new FontIcon() { Glyph = CommonGlyphs.PowerButton },
                Command = CommonCommands.RestartAppCommand
            };

            var restartItem = new MenuItem()
            {
                Header = "Restart",
                ToolTip = "Restart application",
                Icon = new FontIcon() { Glyph = CommonGlyphs.Pause },
                Command = CommonCommands.RestartAppCommand
            };

            var pauseItem = new MenuItem()
            {
                Header = "Pause",
                ToolTip = "Pause application",
                Icon = new FontIcon() { Glyph = CommonGlyphs.Pause },
                Command = CommonCommands.RestartAppCommand
            };

            var resumeItem = new MenuItem()
            {
                Header = "Resume",
                ToolTip = "Resume application",
                Icon = new FontIcon() { Glyph = CommonGlyphs.RepeatOne },
                Command = CommonCommands.RestartAppCommand
            };

            TaskbarIconContextMenu = new ContextMenu()
            {
                Items = { settingsItem, exitItem, restartItem, pauseItem, resumeItem }
            };

            TaskbarIconToolTip = new ToolTip() { Content = "Network Status Monitor" };

            TaskbarIcon = new TaskbarIcon()
            {
                TrayToolTip = TaskbarIconToolTip,
                ContextMenu = TaskbarIconContextMenu,
                DoubleClickCommand = CommonCommands.SettingAppCommand,
                Icon = GetImageByName("NetworkMon.Assets.tray_icon.ico")
            };
        }

        public static Icon GetImageByName(string imageName)
        {
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
            string[] names = asm.GetManifestResourceNames();
            var stream = asm.GetManifestResourceStream($"{imageName}");
            var bmp = Bitmap.FromStream(stream);
            var thumb = (Bitmap)bmp.GetThumbnailImage(64, 64, null, IntPtr.Zero);
            thumb.MakeTransparent();
            return Icon.FromHandle(thumb.GetHicon());
        }
    }
}

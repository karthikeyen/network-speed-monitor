﻿using Hardcodet.Wpf.TaskbarNotification;
using ModernWpf;
using ModernWpf.Controls;
using NetworkMon.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

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
                Icon = new FontIcon() { Glyph = CommonGlyphs.PowerButton },
                Command = CommonCommands.RestartAppCommand
            };

            TaskbarIconContextMenu = new ContextMenu()
            {
                Items = { settingsItem, exitItem, restartItem }
            };

            TaskbarIconToolTip = new ToolTip() { Content = "Network Status Monitor" };

            TaskbarIcon = new TaskbarIcon()
            {
                TrayToolTip = TaskbarIconToolTip,
                ContextMenu = TaskbarIconContextMenu,
                DoubleClickCommand = CommonCommands.OpenSettingsWindowCommand,
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

        public static void UpdateTrayIconVisibility(bool isVisible)
        {
            TaskbarIcon.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public static void UpdateTrayIconInternal(ElementTheme currentTheme, bool useColoredTrayIcon)
        {
            ThemeManager.SetRequestedTheme(TaskbarIconContextMenu, currentTheme);
            ThemeManager.SetRequestedTheme(TaskbarIconToolTip, currentTheme);

            Uri iconUri = null;
            //if (useColoredTrayIcon)
            //{
            //    iconUri = PackUriHelper.GetAbsoluteUri(@"Assets\Logo.ico");
            //}
            //else
            //{
            //    iconUri = PackUriHelper.GetAbsoluteUri(currentTheme == ElementTheme.Light ? @"Assets\Logo_Tray_Black.ico" : @"Assets\Logo_Tray_White.ico");
            //}

            TaskbarIcon.IconSource = BitmapFrame.Create(iconUri);
        }
    }
}
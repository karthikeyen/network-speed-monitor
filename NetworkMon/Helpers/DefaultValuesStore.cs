using ModernWpf;
using NetworkMon.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace NetworkMon.Helpers
{
    internal class DefaultValuesStore
    {
        #region General

        public const bool AudioModuleEnabled = true;

        public const bool BrightnessModuleEnabled = true;

        public const bool AirplaneModeModuleEnabled = true;

        public const bool LockKeysModuleEnabled = true;

        #endregion


        #region Module specific

        #region Audio module related

        public const bool ShowGSMTCInVolumeFlyout = true;

        public const bool ShowVolumeControlInGSMTCFlyout = true;

        #endregion

        #region Lock keys module related

        public const bool LockKeysModule_CapsLockEnabled = true;

        public const bool LockKeysModule_NumLockEnabled = true;

        public const bool LockKeysModule_ScrollLockEnabled = true;

        public const bool LockKeysModule_InsertEnabled = true;

        #endregion

        #endregion

        #region UI

        public const ElementTheme AppTheme = ElementTheme.Default;

        public const ElementTheme FlyoutTheme = ElementTheme.Default;

        public const int FlyoutTimeout = 2750;

        public static string RecommendedFlyoutTimeout = FlyoutTimeout.ToString();

        public const double FlyoutBackgroundOpacity = 100.0;

        public const bool TrayIconEnabled = true;

        public const bool UseColoredTrayIcon = true;

        public const bool AlignGSMTCThumbnailToRight = true;

        public const bool UseGSMTCThumbnailAsBackground = true;

        public const int MaxVerticalSessionControlsCount = 1;

        public const Orientation SessionsPanelOrientation = Orientation.Horizontal;

        public const FlyoutWindowPlacementMode OnScreenFlyoutWindowPlacementMode = FlyoutWindowPlacementMode.Auto;

        public const FlyoutWindowAlignments OnScreenFlyoutWindowAlignment = FlyoutWindowAlignments.Top | FlyoutWindowAlignments.Left;

        public static Thickness OnScreenFlyoutWindowMargin = new Thickness(10);

        public const FlyoutWindowExpandDirection OnScreenFlyoutWindowExpandDirection = FlyoutWindowExpandDirection.Auto;

       // public const StackingDirection OnScreenFlyoutContentStackingDirection = StackingDirection.Ascending;


        #endregion
    }
}

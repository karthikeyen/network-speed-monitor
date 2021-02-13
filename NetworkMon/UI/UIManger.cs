using Microsoft.Toolkit.Mvvm.ComponentModel;
using ModernWpf;
using NetworkMon.Helpers;
using NetworkMon.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;

namespace NetworkMon.UI
{
    public class UIManager : ObservableObject
    {
        public const double FlyoutWidth = 354;

        public const double DefaultSessionControlHeight = 206;

        public const double DefaultSessionsPanelVerticalSpacing = 8;

        public const double FlyoutShadowDepth = 32;

        private ThemeResources themeResources;

        private bool _isThemeUpdated;

        #region Properties

        private bool restartRequired;

        public bool RestartRequired
        {
            get => restartRequired;
            set => SetProperty(ref restartRequired, value);
        }

        #region General

        private ElementTheme appTheme = DefaultValuesStore.AppTheme;

        public ElementTheme AppTheme
        {
            get => appTheme;
            set
            {
                if (SetProperty(ref appTheme, value))
                {
                    // UpdateAppTheme();
                    AppDataHelper.AppTheme = value;
                }
            }
        }

        private ElementTheme flyoutTheme = DefaultValuesStore.FlyoutTheme;

        public ElementTheme FlyoutTheme
        {
            get => flyoutTheme;
            set
            {
                if (SetProperty(ref flyoutTheme, value))
                {
                    // UpdateTheme();
                    AppDataHelper.FlyoutTheme = value;
                }
            }
        }

        private ElementTheme actualFlyoutTheme = ElementTheme.Dark;

        public ElementTheme ActualFlyoutTheme
        {
            get => actualFlyoutTheme;
            private set => SetProperty(ref actualFlyoutTheme, value);
        }

        private int flyoutTimeout = DefaultValuesStore.FlyoutTimeout;

        public int FlyoutTimeout
        {
            get => flyoutTimeout;
            set
            {
                if (SetProperty(ref flyoutTimeout, value))
                {
                    AppDataHelper.FlyoutTimeout = flyoutTimeout;
                }
            }
        }

        private double flyoutBackgroundOpacity = DefaultValuesStore.FlyoutBackgroundOpacity;

        public double FlyoutBackgroundOpacity
        {
            get => flyoutBackgroundOpacity;
            set
            {
                if (SetProperty(ref flyoutBackgroundOpacity, value))
                {
                    OnFlyoutBackgroundOpacityChanged();
                }
            }
        }

        private bool trayIconEnabled = DefaultValuesStore.TrayIconEnabled;

        public bool TrayIconEnabled
        {
            get => trayIconEnabled;
            set
            {
                if (SetProperty(ref trayIconEnabled, value))
                {
                    OnTrayIconEnabledChanged();
                }
            }
        }

        private bool useColoredTrayIcon = DefaultValuesStore.UseColoredTrayIcon;

        public bool UseColoredTrayIcon
        {
            get => useColoredTrayIcon;
            set
            {
                if (SetProperty(ref useColoredTrayIcon, value))
                {
                    OnUseColoredTrayIconChanged();
                }
            }
        }

        #endregion

        #region Layout

        private FlyoutWindowAlignments onScreenFlyoutWindowAlignment;

        public FlyoutWindowAlignments OnScreenFlyoutWindowAlignment
        {
            get => onScreenFlyoutWindowAlignment;
            set
            {
                if (SetProperty(ref onScreenFlyoutWindowAlignment, value))
                {
                    AppDataHelper.OnScreenFlyoutWindowAlignment = value;
                }
            }
        }

        private Thickness onScreenFlyoutWindowMargin;

        public Thickness OnScreenFlyoutWindowMargin
        {
            get => onScreenFlyoutWindowMargin;
            set
            {
                if (SetProperty(ref onScreenFlyoutWindowMargin, value))
                {
                    // AppDataHelper.OnScreenFlyoutWindowMargin = value;
                }
            }
        }

        private FlyoutWindowExpandDirection onScreenFlyoutWindowExpandDirection;

        public FlyoutWindowExpandDirection OnScreenFlyoutWindowExpandDirection
        {
            get => onScreenFlyoutWindowExpandDirection;
            set
            {
                if (SetProperty(ref onScreenFlyoutWindowExpandDirection, value))
                {
                    AppDataHelper.OnScreenFlyoutWindowExpandDirection = value;
                }
            }
        }


        #endregion

        #region Media Controls

        private bool alignGSMTCThumbnailToRight = DefaultValuesStore.AlignGSMTCThumbnailToRight;

        public bool AlignGSMTCThumbnailToRight
        {
            get => alignGSMTCThumbnailToRight;
            set
            {
                if (SetProperty(ref alignGSMTCThumbnailToRight, value))
                {
                    AppDataHelper.AlignGSMTCThumbnailToRight = value;
                }
            }
        }

        private bool useGSMTCThumbnailAsBackground = DefaultValuesStore.UseGSMTCThumbnailAsBackground;

        public bool UseGSMTCThumbnailAsBackground
        {
            get => useGSMTCThumbnailAsBackground;
            set
            {
                if (SetProperty(ref useGSMTCThumbnailAsBackground, value))
                {
                    AppDataHelper.UseGSMTCThumbnailAsBackground = value;
                }
            }
        }

        private int maxVerticalSessionControlsCount = DefaultValuesStore.MaxVerticalSessionControlsCount;

        public int MaxVerticalSessionControlsCount
        {
            get => maxVerticalSessionControlsCount;
            set
            {
                if (SetProperty(ref maxVerticalSessionControlsCount, value))
                {
                    // OnMaxVerticalSessionControlsCount();
                }
            }
        }

        private double calculatedSessionsPanelMaxHeight = DefaultSessionControlHeight;

        public double CalculatedSessionsPanelMaxHeight
        {
            get => calculatedSessionsPanelMaxHeight;
            private set => SetProperty(ref calculatedSessionsPanelMaxHeight, value);
        }

        private double calculatedSessionsPanelSpacing;

        public double CalculatedSessionsPanelSpacing
        {
            get => calculatedSessionsPanelSpacing;
            private set => SetProperty(ref calculatedSessionsPanelSpacing, value);
        }

        #endregion

        #endregion

        public void Initialize()
        {
            OnScreenFlyoutWindowAlignment = AppDataHelper.OnScreenFlyoutWindowAlignment;
            OnScreenFlyoutWindowMargin = AppDataHelper.OnScreenFlyoutWindowMargin;
            OnScreenFlyoutWindowExpandDirection = AppDataHelper.OnScreenFlyoutWindowExpandDirection;
           // OnScreenFlyoutContentStackingDirection = AppDataHelper.OnScreenFlyoutContentStackingDirection;

            FlyoutTimeout = AppDataHelper.FlyoutTimeout;
            AlignGSMTCThumbnailToRight = AppDataHelper.AlignGSMTCThumbnailToRight;
            UseGSMTCThumbnailAsBackground = AppDataHelper.UseGSMTCThumbnailAsBackground;
            MaxVerticalSessionControlsCount = AppDataHelper.MaxVerticalSessionControlsCount;
           // SessionsPanelOrientation = AppDataHelper.SessionsPanelOrientation;


            FlyoutBackgroundOpacity = AppDataHelper.FlyoutBackgroundOpacity;

            TrayIconManager.SetupTrayIcon();

            TrayIconEnabled = AppDataHelper.TrayIconEnabled;
            UseColoredTrayIcon = AppDataHelper.UseColoredTrayIcon;

            FlyoutTheme = AppDataHelper.FlyoutTheme;
            AppTheme = AppDataHelper.AppTheme;

            //SystemTheme.SystemThemeChanged += OnSystemThemeChanged;
            //SystemTheme.Initialize();
        }

        private void OnFlyoutBackgroundOpacityChanged()
        {
            
            AppDataHelper.FlyoutBackgroundOpacity = flyoutBackgroundOpacity;
        }

        private void OnTrayIconEnabledChanged()
        {
            // TrayIconManager.UpdateTrayIconVisibility(trayIconEnabled);
            AppDataHelper.TrayIconEnabled = TrayIconEnabled;
        }

        private void OnUseColoredTrayIconChanged()
        {
            
            AppDataHelper.UseColoredTrayIcon = useColoredTrayIcon;
        }
    }

    public enum TopBarVisibility
    {
        Visible = 0,
        AutoHide = 1,
        Collapsed = 2
    }
}

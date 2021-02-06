using ModernWpf.Controls;
using ModernWpf.Media.Animation;
using NetworkMon.Helpers;
using NetworkMon.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace NetworkMon
{
    public partial class Settings : Window
    {
        private Settings()
        {
            InitializeComponent();

            /* var Theme = ThemeHelper.GetTheme();
            if (Theme == THEME.DARK)
            {
                this.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3B3B3B"));
            }
            else
            {
                this.Background = new SolidColorBrush(Colors.White);
            } */

            _pages.Add("about", typeof(About));
            _pages.Add("personalization", typeof(PersonalizationPage));
            _pages.Add("general", typeof(GeneralPage));

            ContentFrame.Navigated += ContentFrame_Navigated;
            this.Closing += Settings_Closing;

            KeyDown += (s, e) =>
            {
                if (e.Key == Key.Back || (e.Key == Key.Left && Keyboard.Modifiers == ModifierKeys.Alt))
                {
                    BackRequested();
                }
            };

            
        }

        private void Settings_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _instance = null;
        }

        private void ContentFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            NavView.IsBackEnabled = ContentFrame.CanGoBack;
            Type sourcePageType = ContentFrame.SourcePageType;
            if (sourcePageType != null)
            {
                var item = _pages.FirstOrDefault(p => p.Value == sourcePageType);

                NavView.SelectedItem = NavView.FooterMenuItems
                    .OfType<NavigationViewItem>().
                    FirstOrDefault(n => n.Tag.Equals(item.Key)) ??
                    NavView.MenuItems
                    .OfType<NavigationViewItem>()
                    .FirstOrDefault(n => n.Tag.Equals(item.Key));

                HeaderBlock.Text =
                    ((NavigationViewItem)NavView.SelectedItem)?.Content?.ToString();
            }
        }

        private static Settings _instance;
        public static Settings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Settings();
                }

                return _instance;
            }
        }

        private void NavView_SelectionChanged(ModernWpf.Controls.NavigationView sender, ModernWpf.Controls.NavigationViewSelectionChangedEventArgs args)
        {

            if (args.SelectedItem != null)
            {
                var navItemTag = args.SelectedItemContainer.Tag.ToString();
                DoNavigate(navItemTag, args.RecommendedNavigationTransitionInfo);
            }
        }

        private void DoNavigate(string navItemTag, NavigationTransitionInfo info)
        {
            var item = _pages.FirstOrDefault(p => p.Key.Equals(navItemTag));
            Type pageType = item.Value;

            if (pageType != null && ContentFrame.CurrentSourcePageType != pageType)
            {
                ContentFrame.Navigate(pageType, null, info);
            }
        }

        private void NavView_BackRequested(ModernWpf.Controls.NavigationView sender, ModernWpf.Controls.NavigationViewBackRequestedEventArgs args)
        {
            BackRequested();
        }

        private bool BackRequested()
        {
            if (!ContentFrame.CanGoBack) return false;

            if (NavView.IsPaneOpen &&
                (NavView.DisplayMode == NavigationViewDisplayMode.Minimal
                 || NavView.DisplayMode == NavigationViewDisplayMode.Compact))
            {
                return false;
            }

            ContentFrame.GoBack();
            return true;

        }
        private readonly Dictionary<string, Type> _pages = new Dictionary<string, Type>();
    }
}

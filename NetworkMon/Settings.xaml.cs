using ModernWpf.Media.Animation;
using NetworkMon.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace NetworkMon
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
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
        }

        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);
            if (this.IsVisible)
            {
                this.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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

        }

        private static readonly Dictionary<string, Type> _pages = new Dictionary<string, Type>();
    }
}

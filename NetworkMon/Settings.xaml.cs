using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            this.WindowStartupLocation = WindowStartupLocation.Manual;

            this.Left = SystemParameters.PrimaryScreenWidth - 250;
            this.Top = SystemParameters.PrimaryScreenHeight - 425;

            var Theme = ThemeHelper.GetTheme();
            if (Theme == THEME.DARK)
            {
                this.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3B3B3B"));
            }
            else
            {
                this.Background = new SolidColorBrush(Colors.White);
            }
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
    }
}

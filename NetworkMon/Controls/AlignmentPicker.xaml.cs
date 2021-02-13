using NetworkMon.Types;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NetworkMon.Controls
{
    /// <summary>
    /// Interaction logic for AlignmentPicker.xaml
    /// </summary>
    /// 
    public partial class AlignmentPicker : UserControl
    {
        public static readonly DependencyProperty AlignmentProperty =
            DependencyProperty.Register(
                nameof(Alignment),
                typeof(FlyoutWindowAlignments),
                typeof(AlignmentPicker),
                new FrameworkPropertyMetadata(FlyoutWindowAlignments.Top | FlyoutWindowAlignments.Left,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnAlignmentPropertyChanged));

        public FlyoutWindowAlignments Alignment
        {
            get => (FlyoutWindowAlignments)GetValue(AlignmentProperty);
            set => SetValue(AlignmentProperty, value);
        }

        private static void OnAlignmentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AlignmentPicker flyoutAlignmentPicker)
            {
                flyoutAlignmentPicker.UpdateControls((FlyoutWindowAlignments)e.NewValue);
            }
        }

        public AlignmentPicker()
        {
            InitializeComponent();
        }

        private bool _isUpdating;

        private void UpdateControls(FlyoutWindowAlignments alignment)
        {
            _isUpdating = true;

            TgLeft.IsChecked = alignment.HasFlag(FlyoutWindowAlignments.Left);
            TgRight.IsChecked = alignment.HasFlag(FlyoutWindowAlignments.Right);
            TgTop.IsChecked = alignment.HasFlag(FlyoutWindowAlignments.Top);
            TgBottom.IsChecked = alignment.HasFlag(FlyoutWindowAlignments.Bottom);

            TgCenter.IsChecked = alignment == FlyoutWindowAlignments.Center;

            CmbHorizontal.SelectedItem = GetHorizontalAlignment(alignment);
            CmbVertical.SelectedItem = GetVerticalAlignment(alignment);

            _isUpdating = false;
        }

        private FlyoutWindowAlignments GetHorizontalAlignment(FlyoutWindowAlignments alignment)
        {
            if (alignment.HasFlag(FlyoutWindowAlignments.Left))
            {
                return FlyoutWindowAlignments.Left;
            }
            else if (alignment.HasFlag(FlyoutWindowAlignments.Right))
            {
                return FlyoutWindowAlignments.Right;
            }
            else
            {
                return FlyoutWindowAlignments.Center;
            }
        }

        private FlyoutWindowAlignments GetVerticalAlignment(FlyoutWindowAlignments alignment)
        {
            if (alignment.HasFlag(FlyoutWindowAlignments.Top))
            {
                return FlyoutWindowAlignments.Top;
            }
            else if (alignment.HasFlag(FlyoutWindowAlignments.Bottom))
            {
                return FlyoutWindowAlignments.Bottom;
            }
            else
            {
                return FlyoutWindowAlignments.Center;
            }
        }
    }
}

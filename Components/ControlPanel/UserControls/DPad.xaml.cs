using System.Windows;
using System.Windows.Controls;

namespace ControlPanel.UserControls
{
    /// <summary>
    /// Interaction logic for DPad.xaml
    /// </summary>
    public partial class DPad : UserControl
    {
        private enum ButtonType { Top, Bottom, Left, Right }

        public DPad()
        {
            InitializeComponent();
        }

        public string LabelForTopButton
        {
            get => (string)GetValue(TopLabelProperty);
            set => SetValue(TopLabelProperty, value);
        }

        public string LabelFormBottomButton
        {
            get => (string)GetValue(BottomLabelProperty);
            set => SetValue(BottomLabelProperty, value);
        }

        public string LabelForLeftButton
        {
            get => (string)GetValue(LeftLabelProperty);
            set => SetValue(LeftLabelProperty, value);
        }

        public string LabelForRightButton
        {
            get => (string)GetValue(RightLabelProperty);
            set => SetValue(RightLabelProperty, value);
        }

        public static readonly DependencyProperty TopLabelProperty = DependencyProperty.Register(nameof(TopLabelProperty), typeof(string), typeof(DPad), new PropertyMetadata(string.Empty, (s, e) => LabelChanged(s, e, ButtonType.Top)));
        public static readonly DependencyProperty BottomLabelProperty = DependencyProperty.Register(nameof(BottomLabelProperty), typeof(string), typeof(DPad), new PropertyMetadata(string.Empty, (s, e) => LabelChanged(s, e, ButtonType.Bottom)));
        public static readonly DependencyProperty LeftLabelProperty = DependencyProperty.Register(nameof(LeftLabelProperty), typeof(string), typeof(DPad), new PropertyMetadata(string.Empty, (s, e) => LabelChanged(s, e, ButtonType.Left)));
        public static readonly DependencyProperty RightLabelProperty = DependencyProperty.Register(nameof(RightLabelProperty), typeof(string), typeof(DPad), new PropertyMetadata(string.Empty, (s, e) => LabelChanged(s, e, ButtonType.Right)));

        private static void LabelChanged(DependencyObject sender, DependencyPropertyChangedEventArgs eventArgs, ButtonType buttonType)
        {
            GetDPadButton((DPad)sender, buttonType).Label = (string)eventArgs.NewValue;
        }

        private static Button GetDPadButton(DPad dpad, ButtonType buttonType)
        {
            return buttonType switch
            {
                ButtonType.Top => dpad.TopButton,
                ButtonType.Bottom => dpad.BottomButton,
                ButtonType.Left => dpad.LeftButton,
                ButtonType.Right => dpad.RightButton,
                _ => throw new NotImplementedException(),
            };
        }
    }
}

using System.Windows;
using System.Windows.Controls;

namespace ControlPanel.UserControls
{
    /// <summary>
    /// Interaction logic for Button.xaml
    /// </summary>
    public partial class Button : UserControl
    {
        public Button()
        {
            InitializeComponent();
        }

        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public bool Pressed
        {
            get => (bool)GetValue(PressedProperty);
            set => SetValue(PressedProperty, value);
        }

        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(Button), new PropertyMetadata(string.Empty, LabelChanged));

        public static readonly DependencyProperty PressedProperty = DependencyProperty.Register("Pressed", typeof(bool), typeof(Button), new PropertyMetadata(false, ValueChanged));

        private static void LabelChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((Button)sender).ButtonLabel.Content = (string)e.NewValue;
        }

        private static void ValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((Button)sender).PressedIndicator.Visibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Hidden;
        }
    }
}

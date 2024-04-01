using Common.Extensions;
using ControlPanel.Extensions;
using System.Windows;
using System.Windows.Controls;

namespace ControlPanel.UserControls
{
    /// <summary>
    /// Interaction logic for Stick.xaml
    /// </summary>
    public partial class Stick : UserControl
    {
        public Stick()
        {
            InitializeComponent();
        }

        public Vector Vector
        {
            get => (Vector)GetValue(VectorProperty);
            set => SetValue(VectorProperty, value);
        }

        public static readonly DependencyProperty VectorProperty = DependencyProperty.Register("Vector", typeof(Vector), typeof(Stick), new PropertyMetadata(new Vector(), VectorChanged), new ValidateValueCallback(IsValidVector));

        private static void VectorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var stick = (Stick)sender;
            var vector = (Vector)e.NewValue;
            stick.Knob.Margin = vector.Normalize(stick).AsThickness();
        }

        private static bool IsValidVector(object value) => value is Vector vector && vector.X.IsBetween(-1, 1) && vector.Y.IsBetween(-1, 1);
    }
}

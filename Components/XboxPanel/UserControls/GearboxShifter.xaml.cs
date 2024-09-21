using KrzaqTools.Extensions;
using System.Windows;
using System.Windows.Controls;
using XboxPanel.Extensions;

namespace ControlPanel.UserControls
{
    /// <summary>
    /// Interaction logic for Shifter.xaml
    /// </summary>
    public partial class GearboxShifter : UserControl
    {
        public enum Gear
        {
            Reverse = -1,
            Neutral,
            First,
            Second,
            Third,
            Fourth,
            Fifth
        }

        public GearboxShifter()
        {
            InitializeComponent();
        }

        public Vector Vector
        {
            get => (Vector)GetValue(VectorProperty);
            set => SetValue(VectorProperty, value);
        }

        public static readonly DependencyProperty VectorProperty = DependencyProperty.Register("Vector", typeof(Vector), typeof(GearboxShifter), new PropertyMetadata(new Vector(), VectorChanged), new ValidateValueCallback(IsValidVector));

        private static void VectorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var shifter = (GearboxShifter)sender;
            var vector = (Vector)e.NewValue;
            shifter.Knob.Margin = vector.Normalize(shifter).AsThickness();
        }

        private static bool IsValidVector(object value) => value is Vector vector && vector.X.IsBetween(-1, 1) && vector.Y.IsBetween(-1, 1);
    }
}

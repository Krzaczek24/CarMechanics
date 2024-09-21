using KrzaqTools.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Throttle.Constants;

namespace Throttle.UserControls
{
    /// <summary>
    /// Interaction logic for Throttle.xaml
    /// </summary>
    public partial class Throttle : UserControl
    {
        public Throttle()
        {
            InitializeComponent();
        }

        public double Angle
        {
            get => (double)GetValue(AngleProperty);
            set => SetValue(AngleProperty, value);
        }

        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register(nameof(AngleProperty), typeof(double), typeof(Throttle), new PropertyMetadata((double)ThrottleAngles.MINIMAL, AngleChanged), new ValidateValueCallback(IsValidAngle));

        private static void AngleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var throttle = (Throttle)sender;
            double angle = (double)e.NewValue;
            throttle.ThrottlePlane.RenderTransform = new RotateTransform(angle);
        }

        private static bool IsValidAngle(object value) => value is double angle && angle.IsBetween(ThrottleAngles.MINIMAL, ThrottleAngles.MAXIMAL);
    }
}

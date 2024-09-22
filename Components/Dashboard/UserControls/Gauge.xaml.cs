using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Common;
using Common.WPF;

namespace Dashboard.UserControls
{
    /// <summary>
    /// Interaction logic for Gauge.xaml
    /// </summary>
    public partial class Gauge : UserControl
    {
        private const byte ByteZero = 0;
        private const byte MinScaleMajorGridLines = 2;
        private const byte MinScaleMinorGridLines = 0;
        private const byte DefaultScaleMajorGridLines = 9;
        private const byte DefaultScaleMinorGridLines = 3;
        private const byte DefaultScaleMajorGridLineLength = 6;
        private const byte DefaultScaleMinorGridLineLength = 4;
        private const byte DefaultIndicatorLength = 36;
        private const byte DefaultIndicatorThickness = 3;

        private static readonly Brush DefaultScaleBrush = Brushes.Lime;
        private static readonly Brush DefaultIndicatorBrush = Brushes.Wheat;

        private const double Angle = (Math.PI / 180) * 120;
        private const double HalfSize = 50;
        private const double OuterScaleRadius = HalfSize - 4;

        public Gauge()
        {
            InitializeComponent();
            DrawGauge();
        }

        public Brush ScaleBrush
        {
            get => (Brush)GetValue(ScaleBrushProperty);
            set => SetValue(ScaleBrushProperty, value);
        }

        public int ScaleMinValue
        {
            get => (int)GetValue(ScaleMinValueProperty);
            set => SetValue(ScaleMinValueProperty, value);
        }

        public int ScaleMaxValue
        {
            get => (int)GetValue(ScaleMaxValueProperty);
            set => SetValue(ScaleMaxValueProperty, value);
        }

        public byte ScaleMajorGridLines
        {
            get => (byte)GetValue(ScaleMajorGridLinesProperty);
            set => SetValue(ScaleMajorGridLinesProperty, value);
        }

        public byte ScaleMajorGridLineLength
        {
            get => (byte)GetValue(ScaleMajorGridLineLengthProperty);
            set => SetValue(ScaleMajorGridLineLengthProperty, value);
        }

        public byte ScaleMinorGridLines
        {
            get => (byte)GetValue(ScaleMinorGridLinesProperty);
            set => SetValue(ScaleMinorGridLinesProperty, value);
        }

        public byte ScaleMinorGridLineLength
        {
            get => (byte)GetValue(ScaleMinorGridLineLengthProperty);
            set => SetValue(ScaleMinorGridLineLengthProperty, value);
        }

        public Brush IndicatorBrush
        {
            get => (Brush)GetValue(IndicatorBrushProperty);
            set => SetValue(IndicatorBrushProperty, value);
        }

        public byte IndicatorLength
        {
            get => (byte)GetValue(IndicatorLengthProperty);
            set => SetValue(IndicatorLengthProperty, value);
        }

        public byte IndicatorThickness
        {
            get => (byte)GetValue(IndicatorThicknessProperty);
            set => SetValue(IndicatorThicknessProperty, value);
        }

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly DependencyProperty ScaleBrushProperty = DependencyProperty.Register(nameof(ScaleBrushProperty), typeof(Brush), typeof(Gauge), new PropertyMetadata(DefaultScaleBrush, GaugePropsChanged));
        public static readonly DependencyProperty ScaleMinValueProperty = DependencyProperty.Register(nameof(ScaleMinValueProperty), typeof(int), typeof(Gauge), new PropertyMetadata(0, GaugePropsChanged));
        public static readonly DependencyProperty ScaleMaxValueProperty = DependencyProperty.Register(nameof(ScaleMaxValueProperty), typeof(int), typeof(Gauge), new PropertyMetadata(100, GaugePropsChanged));
        public static readonly DependencyProperty ScaleMajorGridLinesProperty = DependencyProperty.Register(nameof(ScaleMajorGridLinesProperty), typeof(byte), typeof(Gauge), new PropertyMetadata(DefaultScaleMajorGridLines, GaugePropsChanged), value => value is >= MinScaleMajorGridLines);
        public static readonly DependencyProperty ScaleMinorGridLinesProperty = DependencyProperty.Register(nameof(ScaleMinorGridLinesProperty), typeof(byte), typeof(Gauge), new PropertyMetadata(DefaultScaleMinorGridLines, GaugePropsChanged), value => value is >= MinScaleMinorGridLines);
        public static readonly DependencyProperty ScaleMajorGridLineLengthProperty = DependencyProperty.Register(nameof(ScaleMajorGridLineLengthProperty), typeof(byte), typeof(Gauge), new PropertyMetadata(DefaultScaleMajorGridLineLength, GaugePropsChanged));
        public static readonly DependencyProperty ScaleMinorGridLineLengthProperty = DependencyProperty.Register(nameof(ScaleMinorGridLineLengthProperty), typeof(byte), typeof(Gauge), new PropertyMetadata(DefaultScaleMinorGridLineLength, GaugePropsChanged));
        public static readonly DependencyProperty IndicatorBrushProperty = DependencyProperty.Register(nameof(IndicatorBrushProperty), typeof(Brush), typeof(Gauge), new PropertyMetadata(DefaultIndicatorBrush, GaugePropsChanged));
        public static readonly DependencyProperty IndicatorLengthProperty = DependencyProperty.Register(nameof(IndicatorLengthProperty), typeof(byte), typeof(Gauge), new PropertyMetadata(DefaultIndicatorLength, GaugePropsChanged), value => value is > ByteZero);
        public static readonly DependencyProperty IndicatorThicknessProperty = DependencyProperty.Register(nameof(IndicatorThicknessProperty), typeof(byte), typeof(Gauge), new PropertyMetadata(DefaultIndicatorThickness, GaugePropsChanged), value => value is > ByteZero);
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(ValueProperty), typeof(double), typeof(Gauge), new PropertyMetadata(0d, ValueChanged));

        private static void GaugePropsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => ((Gauge)sender).DrawGauge();

        private static void ValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var gauge = (Gauge)sender;
            double angle = (double)e.NewValue;
        }

        private void DrawGauge()
        {
            GaugeShield.Children.Clear();
            DrawScaleArc();
            DrawScaleGrid();
            DrawIndicatorArrow();
        }

        private void DrawScaleArc()
        {
            double xShift = OuterScaleRadius * Math.Sin(Angle);
            double yShift = OuterScaleRadius * Math.Cos(Angle);

            var startPoint = new Point { X = HalfSize - xShift, Y = HalfSize - yShift };
            var endPoint = startPoint with { X = HalfSize + xShift };
            var size = new Size(OuterScaleRadius, OuterScaleRadius);

            GaugeShield.DrawSimpleArc(startPoint, endPoint, size, ScaleBrush, 2);
        }

        private void DrawScaleGrid()
        {
            DrawScaleMajorGrid();
            DrawScaleMinorGrid();
            DrawScaleValues();
        }

        private void DrawScaleMajorGrid()
        {
            if (ScaleMajorGridLineLength == 0)
                return;

            double shift = 2 * Angle / (ScaleMajorGridLines - 1);
            double innerScaleRadius = OuterScaleRadius - ScaleMajorGridLineLength;
            for (int i = 0; i < ScaleMajorGridLines; i++)
            {
                double angle = Angle - i * shift;
                double sinus = Math.Sin(angle);
                double cosine = Math.Cos(angle);

                var gridLine = new Line
                {
                    Stroke = ScaleBrush,
                    StrokeThickness = 2,
                    X1 = HalfSize - OuterScaleRadius * sinus,
                    Y1 = HalfSize - OuterScaleRadius * cosine,
                    X2 = HalfSize - innerScaleRadius * sinus,
                    Y2 = HalfSize - innerScaleRadius * cosine,
                    StrokeEndLineCap = PenLineCap.Triangle,
                    StrokeStartLineCap = PenLineCap.Square,
                };

                GaugeShield.Children.Add(gridLine);
            }
        }

        private void DrawScaleMinorGrid()
        {
            if (ScaleMinorGridLines == 0)
                return;

            double count = (ScaleMajorGridLines - 1) * (ScaleMinorGridLines + 1);
            double shift = 2 * Angle / count;
            double innerScaleRadius = OuterScaleRadius - ScaleMinorGridLineLength;
            for (int i = 0; i <= count; i++)
            {
                double angle = Angle - i * shift;
                double sinus = Math.Sin(angle);
                double cosine = Math.Cos(angle);

                var gridLine = new Line
                {
                    Stroke = ScaleBrush,
                    StrokeThickness = 1,
                    X1 = HalfSize - OuterScaleRadius * sinus,
                    Y1 = HalfSize - OuterScaleRadius * cosine,
                    X2 = HalfSize - innerScaleRadius * sinus,
                    Y2 = HalfSize - innerScaleRadius * cosine,
                    StrokeEndLineCap = PenLineCap.Triangle,
                    StrokeStartLineCap = PenLineCap.Square,
                };

                GaugeShield.Children.Add(gridLine);
            }
        }

        private void DrawScaleValues()
        {

        }

        private void DrawIndicatorArrow()
        {
            double sinus = Math.Sin(Angle);
            double cosine = Math.Cos(Angle);

            var indicatorArrow = new Line
            {
                Stroke = IndicatorBrush,
                StrokeThickness = IndicatorThickness,
                X1 = HalfSize + 3 * sinus,
                Y1 = HalfSize + 3 * cosine,
                X2 = HalfSize - IndicatorLength * sinus,
                Y2 = HalfSize - IndicatorLength * cosine,
                StrokeEndLineCap = PenLineCap.Triangle,
                StrokeStartLineCap = PenLineCap.Square,
            };

            GaugeShield.Children.Add(indicatorArrow);
        }
    }
}

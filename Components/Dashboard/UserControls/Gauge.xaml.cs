using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using Common;
using Common.WPF;
using KrzaqTools.Extensions;
using Newtonsoft.Json.Linq;

namespace Dashboard.UserControls
{
    /// <summary>
    /// Interaction logic for Gauge.xaml
    /// </summary>
    public partial class Gauge : UserControl
    {
        private const double DefaultScaleAngle = 120;
        private const int MinScaleMajorGridLines = 2;
        private const int MinScaleMinorGridLines = 0;
        private const double DefaultScaleRangeMargin = 4;
        private const int DefaultScaleMajorGridLines = 9;
        private const double DefaultScaleMajorGridLineLength = 6;
        private const int DefaultScaleMinorGridLines = 3;
        private const double DefaultScaleMinorGridLineLength = 4;
        private const double DefaultIndicatorThickness = 3;
        private const double DefaultIndicatorRangeEnd = 36;
        private const double DefaultIndicatorRangeStart = -3;
        private const double DefaultOuterBorderThickness = 2;
        private const double DefaultCentralPointSize = 8;
        private const bool DefaultCentralPointOnTop = false;
        private const bool DefaultShowScaleValues = false;
        private const double DefaultScaleMinValue = 0;
        private const double DefaultScaleMaxValue = 100;
        private const double DefaultScaleValuesSize = 8;
        private const double DefaultScaleValuesRadius = 30;
        private const double DefaultScaleValuesXShift = 0;
        private const double DefaultScaleValuesYShift = 0;
        private const double DefaultScaleValuesYMultiplier = 1.05;
        private const double DefaultTextYShift = 20;
        private const double DefaultTextSize = 8;

        private static readonly Brush DefaultScaleBrush = Brushes.Lime;
        private static readonly Brush DefaultIndicatorBrush = Brushes.Wheat;
        private static readonly Brush DefaultOuterBorderBrush = Brushes.Gray;
        private static readonly Brush DefaultCentralPointBrush = Brushes.Gray;
        private static readonly Brush DefaultScaleValuesBrush = Brushes.White;
        private static readonly Brush DefaultTextBrush = Brushes.Gray;

        private static readonly FontFamily DefaultScaleValuesFont = new("Gadugi");
        private static readonly FontFamily DefaultTextFont = new("Gadugi");

        private const double DegreesRadianFactor = Math.PI / 180;
        private const double HalfSize = 50;

        public Gauge()
        {
            InitializeComponent();
            DrawGauge();
        }

        private Line? Indicator { get; set; }

        public double OuterBorderThickness
        {
            get => (double)GetValue(OuterBorderThicknessProperty);
            set => SetValue(OuterBorderThicknessProperty, value);
        }

        public Brush OuterBorderBrush
        {
            get => (Brush)GetValue(OuterBorderBrushProperty);
            set => SetValue(OuterBorderBrushProperty, value);
        }

        public Brush CentralPointBrush
        {
            get => (Brush)GetValue(CentralPointBrushProperty);
            set => SetValue(CentralPointBrushProperty, value);
        }

        public bool CentralPointOnTop
        {
            get => (bool)GetValue(CentralPointOnTopProperty);
            set => SetValue(CentralPointOnTopProperty, value);
        }

        private double OuterScaleRadius { get; set; }
        public double CentralPointSize
        {
            get => (double)GetValue(CentralPointSizeProperty);
            set => SetValue(CentralPointSizeProperty, value);
        }

        public Brush ScaleBrush
        {
            get => (Brush)GetValue(ScaleBrushProperty);
            set => SetValue(ScaleBrushProperty, value);
        }

        public double ScaleRangeMargin
        {
            get => (double)GetValue(ScaleRangeMarginProperty);
            set => SetValue(ScaleRangeMarginProperty, value);
        }

        private double ScaleAngleStartRadians { get; set; }
        public double ScaleAngleStart
        {
            get => (double)GetValue(ScaleAngleStartProperty);
            set => SetValue(ScaleAngleStartProperty, value);
        }

        private double ScaleAngleEndRadians { get; set; }
        public double ScaleAngleEnd
        {
            get => (double)GetValue(ScaleAngleEndProperty);
            set => SetValue(ScaleAngleEndProperty, value);
        }

        public bool ShowScaleValues
        {
            get => (bool)GetValue(ShowScaleValuesProperty);
            set => SetValue(ShowScaleValuesProperty, value);
        }

        public FontFamily ScaleValuesFont
        {
            get => (FontFamily)GetValue(ScaleValuesFontProperty);
            set => SetValue(ScaleValuesFontProperty, value);
        }

        public double ScaleValuesXShift
        {
            get => (double)GetValue(ScaleValuesXShiftProperty);
            set => SetValue(ScaleValuesXShiftProperty, value);
        }

        public double ScaleValuesYShift
        {
            get => (double)GetValue(ScaleValuesYShiftProperty);
            set => SetValue(ScaleValuesYShiftProperty, value);
        }

        public double ScaleValuesYMultiplier
        {
            get => (double)GetValue(ScaleValuesYMultiplierProperty);
            set => SetValue(ScaleValuesYMultiplierProperty, value);
        }

        public Brush ScaleValuesBrush
        {
            get => (Brush)GetValue(ScaleValuesBrushProperty);
            set => SetValue(ScaleValuesBrushProperty, value);
        }

        public double ScaleValuesSize
        {
            get => (double)GetValue(ScaleValuesSizeProperty);
            set => SetValue(ScaleValuesSizeProperty, value);
        }

        public double ScaleValuesRadius
        {
            get => (double)GetValue(ScaleValuesRadiusProperty);
            set => SetValue(ScaleValuesRadiusProperty, value);
        }

        public double ScaleMinValue
        {
            get => (double)GetValue(ScaleMinValueProperty);
            set => SetValue(ScaleMinValueProperty, value);
        }

        public double ScaleMaxValue
        {
            get => (double)GetValue(ScaleMaxValueProperty);
            set => SetValue(ScaleMaxValueProperty, value);
        }

        public int ScaleMajorGridLines
        {
            get => (int)GetValue(ScaleMajorGridLinesProperty);
            set => SetValue(ScaleMajorGridLinesProperty, value);
        }

        public double ScaleMajorGridLineLength
        {
            get => (double)GetValue(ScaleMajorGridLineLengthProperty);
            set => SetValue(ScaleMajorGridLineLengthProperty, value);
        }

        public int ScaleMinorGridLines
        {
            get => (int)GetValue(ScaleMinorGridLinesProperty);
            set => SetValue(ScaleMinorGridLinesProperty, value);
        }

        public double ScaleMinorGridLineLength
        {
            get => (double)GetValue(ScaleMinorGridLineLengthProperty);
            set => SetValue(ScaleMinorGridLineLengthProperty, value);
        }

        public Brush IndicatorBrush
        {
            get => (Brush)GetValue(IndicatorBrushProperty);
            set => SetValue(IndicatorBrushProperty, value);
        }

        public double IndicatorOuterRange
        {
            get => (double)GetValue(IndicatorOuterRangeProperty);
            set => SetValue(IndicatorOuterRangeProperty, value);
        }

        public double IndicatorInnerRange
        {
            get => (double)GetValue(IndicatorInnerRangeProperty);
            set => SetValue(IndicatorInnerRangeProperty, value);
        }

        public double IndicatorThickness
        {
            get => (double)GetValue(IndicatorThicknessProperty);
            set => SetValue(IndicatorThicknessProperty, value);
        }

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public double TextYShift
        {
            get => (double)GetValue(TextYShiftProperty);
            set => SetValue(TextYShiftProperty, value);
        }

        public double TextSize
        {
            get => (double)GetValue(TextSizeProperty);
            set => SetValue(TextSizeProperty, value);
        }

        public Brush TextBrush
        {
            get => (Brush)GetValue(TextBrushProperty);
            set => SetValue(TextBrushProperty, value);
        }

        public FontFamily TextFont
        {
            get => (FontFamily)GetValue(TextFontProperty);
            set => SetValue(TextFontProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty OuterBorderThicknessProperty = DependencyProperty.Register(nameof(OuterBorderThicknessProperty), typeof(double), typeof(Gauge), new PropertyMetadata(DefaultOuterBorderThickness, OuterBorderPropsChanged));
        public static readonly DependencyProperty OuterBorderBrushProperty = DependencyProperty.Register(nameof(OuterBorderBrushProperty), typeof(Brush), typeof(Gauge), new PropertyMetadata(DefaultOuterBorderBrush, OuterBorderPropsChanged));
        public static readonly DependencyProperty CentralPointBrushProperty = DependencyProperty.Register(nameof(CentralPointBrushProperty), typeof(Brush), typeof(Gauge), new PropertyMetadata(DefaultCentralPointBrush, CentralPointPropsChanged));
        public static readonly DependencyProperty CentralPointSizeProperty = DependencyProperty.Register(nameof(CentralPointSizeProperty), typeof(double), typeof(Gauge), new PropertyMetadata(DefaultCentralPointSize, CentralPointPropsChanged));
        public static readonly DependencyProperty CentralPointOnTopProperty = DependencyProperty.Register(nameof(CentralPointOnTopProperty), typeof(bool), typeof(Gauge), new PropertyMetadata(DefaultCentralPointOnTop, CentralPointPropsChanged));
        public static readonly DependencyProperty ScaleRangeMarginProperty = DependencyProperty.Register(nameof(ScaleRangeMarginProperty), typeof(double), typeof(Gauge), new PropertyMetadata(DefaultScaleRangeMargin, GaugeScalePropsChanged));
        public static readonly DependencyProperty ScaleBrushProperty = DependencyProperty.Register(nameof(ScaleBrushProperty), typeof(Brush), typeof(Gauge), new PropertyMetadata(DefaultScaleBrush, GaugeScalePropsChanged));
        public static readonly DependencyProperty ScaleAngleStartProperty = DependencyProperty.Register(nameof(ScaleAngleStartProperty), typeof(double), typeof(Gauge), new PropertyMetadata(DefaultScaleAngle, GaugeScalePropsChanged), angle => angle is >= -360d and <= 360d);
        public static readonly DependencyProperty ScaleAngleEndProperty = DependencyProperty.Register(nameof(ScaleAngleEndProperty), typeof(double), typeof(Gauge), new PropertyMetadata(-DefaultScaleAngle, GaugeScalePropsChanged), angle => angle is >= -360d and <= 360d);
        public static readonly DependencyProperty ShowScaleValuesProperty = DependencyProperty.Register(nameof(ShowScaleValuesProperty), typeof(bool), typeof(Gauge), new PropertyMetadata(DefaultShowScaleValues, GaugeScalePropsChanged));
        public static readonly DependencyProperty ScaleValuesFontProperty = DependencyProperty.Register(nameof(ScaleValuesFontProperty), typeof(FontFamily), typeof(Gauge), new PropertyMetadata(DefaultScaleValuesFont, GaugeScalePropsChanged));
        public static readonly DependencyProperty ScaleValuesXShiftProperty = DependencyProperty.Register(nameof(ScaleValuesXShiftProperty), typeof(double), typeof(Gauge), new PropertyMetadata(DefaultScaleValuesXShift, GaugeScalePropsChanged));
        public static readonly DependencyProperty ScaleValuesYShiftProperty = DependencyProperty.Register(nameof(ScaleValuesYShiftProperty), typeof(double), typeof(Gauge), new PropertyMetadata(DefaultScaleValuesYShift, GaugeScalePropsChanged));
        public static readonly DependencyProperty ScaleValuesYMultiplierProperty = DependencyProperty.Register(nameof(ScaleValuesYMultiplierProperty), typeof(double), typeof(Gauge), new PropertyMetadata(DefaultScaleValuesYMultiplier, GaugeScalePropsChanged));
        public static readonly DependencyProperty ScaleValuesBrushProperty = DependencyProperty.Register(nameof(ScaleValuesBrushProperty), typeof(Brush), typeof(Gauge), new PropertyMetadata(DefaultScaleValuesBrush, GaugeScalePropsChanged));
        public static readonly DependencyProperty ScaleValuesSizeProperty = DependencyProperty.Register(nameof(ScaleValuesSizeProperty), typeof(double), typeof(Gauge), new PropertyMetadata(DefaultScaleValuesSize, GaugeScalePropsChanged), value => value is > 0d);
        public static readonly DependencyProperty ScaleValuesRadiusProperty = DependencyProperty.Register(nameof(ScaleValuesRadiusProperty), typeof(double), typeof(Gauge), new PropertyMetadata(DefaultScaleValuesRadius, GaugeScalePropsChanged), value => value is > 0d);
        public static readonly DependencyProperty ScaleMinValueProperty = DependencyProperty.Register(nameof(ScaleMinValueProperty), typeof(double), typeof(Gauge), new PropertyMetadata(DefaultScaleMinValue, GaugeScalePropsChanged));
        public static readonly DependencyProperty ScaleMaxValueProperty = DependencyProperty.Register(nameof(ScaleMaxValueProperty), typeof(double), typeof(Gauge), new PropertyMetadata(DefaultScaleMaxValue, GaugeScalePropsChanged));
        public static readonly DependencyProperty ScaleMajorGridLinesProperty = DependencyProperty.Register(nameof(ScaleMajorGridLinesProperty), typeof(int), typeof(Gauge), new PropertyMetadata(DefaultScaleMajorGridLines, GaugeScalePropsChanged), value => value is >= MinScaleMajorGridLines);
        public static readonly DependencyProperty ScaleMinorGridLinesProperty = DependencyProperty.Register(nameof(ScaleMinorGridLinesProperty), typeof(int), typeof(Gauge), new PropertyMetadata(DefaultScaleMinorGridLines, GaugeScalePropsChanged), value => value is >= MinScaleMinorGridLines);
        public static readonly DependencyProperty ScaleMajorGridLineLengthProperty = DependencyProperty.Register(nameof(ScaleMajorGridLineLengthProperty), typeof(double), typeof(Gauge), new PropertyMetadata(DefaultScaleMajorGridLineLength, GaugeScalePropsChanged));
        public static readonly DependencyProperty ScaleMinorGridLineLengthProperty = DependencyProperty.Register(nameof(ScaleMinorGridLineLengthProperty), typeof(double), typeof(Gauge), new PropertyMetadata(DefaultScaleMinorGridLineLength, GaugeScalePropsChanged));
        public static readonly DependencyProperty IndicatorBrushProperty = DependencyProperty.Register(nameof(IndicatorBrushProperty), typeof(Brush), typeof(Gauge), new PropertyMetadata(DefaultIndicatorBrush, GaugeScalePropsChanged));
        public static readonly DependencyProperty IndicatorOuterRangeProperty = DependencyProperty.Register(nameof(IndicatorOuterRangeProperty), typeof(double), typeof(Gauge), new PropertyMetadata(DefaultIndicatorRangeEnd, GaugeScalePropsChanged), value => value is > 0d);
        public static readonly DependencyProperty IndicatorInnerRangeProperty = DependencyProperty.Register(nameof(IndicatorInnerRangeProperty), typeof(double), typeof(Gauge), new PropertyMetadata(DefaultIndicatorRangeStart, GaugeScalePropsChanged));
        public static readonly DependencyProperty IndicatorThicknessProperty = DependencyProperty.Register(nameof(IndicatorThicknessProperty), typeof(double), typeof(Gauge), new PropertyMetadata(DefaultIndicatorThickness, GaugeScalePropsChanged), value => value is > 0d);
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(ValueProperty), typeof(double), typeof(Gauge), new PropertyMetadata(DefaultScaleMinValue, ValueChanged));
        public static readonly DependencyProperty TextYShiftProperty = DependencyProperty.Register(nameof(TextYShiftProperty), typeof(double), typeof(Gauge), new PropertyMetadata(DefaultTextYShift, GaugeScalePropsChanged));
        public static readonly DependencyProperty TextSizeProperty = DependencyProperty.Register(nameof(TextSizeProperty), typeof(double), typeof(Gauge), new PropertyMetadata(DefaultTextSize, GaugeScalePropsChanged));
        public static readonly DependencyProperty TextBrushProperty = DependencyProperty.Register(nameof(TextBrushProperty), typeof(Brush), typeof(Gauge), new PropertyMetadata(DefaultTextBrush, GaugeScalePropsChanged));
        public static readonly DependencyProperty TextFontProperty = DependencyProperty.Register(nameof(TextFontProperty), typeof(FontFamily), typeof(Gauge), new PropertyMetadata(DefaultTextFont, GaugeScalePropsChanged));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(TextProperty), typeof(string), typeof(Gauge), new PropertyMetadata(string.Empty, GaugeScalePropsChanged));

        private static void OuterBorderPropsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is not Gauge gauge) return;
            var outerBorder = gauge.OuterBorder;
            outerBorder.StrokeThickness = gauge.OuterBorderThickness;
            outerBorder.Stroke = gauge.OuterBorderBrush;
        }

        private static void CentralPointPropsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is not Gauge gauge) return;
            var centralPoint = gauge.CentralPoint;
            centralPoint.StrokeThickness = gauge.CentralPointSize / 2d;
            centralPoint.Width = gauge.CentralPointSize;
            centralPoint.Height = gauge.CentralPointSize;
            centralPoint.RenderTransform = new TranslateTransform(-gauge.CentralPointSize / 2d, -gauge.CentralPointSize / 2d);
            centralPoint.Stroke = gauge.CentralPointBrush;
            Panel.SetZIndex(centralPoint, gauge.CentralPointOnTop ? 100 : 0);
        }

        private static void GaugeScalePropsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is not Gauge gauge) return;
            gauge.ScaleAngleStartRadians = DegToRad(gauge.ScaleAngleStart);
            gauge.ScaleAngleEndRadians = DegToRad(gauge.ScaleAngleEnd);
            gauge.OuterScaleRadius = HalfSize - gauge.ScaleRangeMargin;
            gauge.DrawGauge();
            return;

            static double DegToRad(double degrees) => degrees * DegreesRadianFactor;
        }

        private static void ValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is not double newValue || sender is not Gauge gauge) return;
            double newAngle = newValue.Scale(gauge.ScaleMinValue, gauge.ScaleMaxValue, 0, gauge.ScaleAngleStart - gauge.ScaleAngleEnd);
            gauge.Indicator!.RenderTransform = new RotateTransform(newAngle, HalfSize, HalfSize);
        }

        private void DrawGauge()
        {
            GaugeShield.Children.Clear();
            DrawScaleArc();
            DrawScaleMajorGrid();
            DrawScaleMinorGrid();
            DrawScaleValue();
            DrawIndicatorArrow();
            DrawText();
        }

        private void DrawScaleArc()
        {
            var startPoint = new Point
            {
                X = HalfSize - OuterScaleRadius * Math.Sin(ScaleAngleStartRadians),
                Y = HalfSize - OuterScaleRadius * Math.Cos(ScaleAngleStartRadians)
            };
            var endPoint = new Point
            {
                X = HalfSize - OuterScaleRadius * Math.Sin(ScaleAngleEndRadians),
                Y = HalfSize - OuterScaleRadius * Math.Cos(ScaleAngleEndRadians)
            };
            var size = new Size(OuterScaleRadius, OuterScaleRadius);

            GaugeShield.DrawSimpleArc(startPoint, endPoint, size, ScaleBrush, 2);
        }

        private void DrawScaleMajorGrid()
        {
            if (ScaleMajorGridLineLength == 0) return;

            double angleShift = (ScaleAngleStartRadians - ScaleAngleEndRadians) / (ScaleMajorGridLines - 1);
            double innerScaleRadius = OuterScaleRadius - ScaleMajorGridLineLength;

            for (int i = 0; i < ScaleMajorGridLines; i++)
            {
                double angle = ScaleAngleStartRadians - i * angleShift;
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

        private void DrawScaleValue()
        {
            if (!ShowScaleValues) return;

            double valueShift = (ScaleMaxValue - ScaleMinValue) / (ScaleMajorGridLines - 1);
            double angleShift = (ScaleAngleStartRadians - ScaleAngleEndRadians) / (ScaleMajorGridLines - 1);
            double digitWidth = ScaleValuesSize * 0.6;
            double height = ScaleValuesSize * 1.4;
            double yRadius = ScaleValuesRadius * ScaleValuesYMultiplier;

            for (int i = 0; i < ScaleMajorGridLines; i++)
            {
                string value = Math.Round(ScaleMinValue + i * valueShift, MidpointRounding.AwayFromZero).ToString();
                double angle = ScaleAngleStartRadians - i * angleShift;
                double width = digitWidth * value.Length;

                var label = new TextBlock
                {
                    Foreground = ScaleValuesBrush,
                    FontSize = ScaleValuesSize,
                    TextAlignment = TextAlignment.Center,
                    Width = width,
                    Height = height,
                    Text = value,
                    FontFamily = ScaleValuesFont,
                    RenderTransform = new TranslateTransform
                    {
                        X = -width * (Math.Sin(angle + Math.PI) + 1) / 2 + ScaleValuesXShift,
                        Y = -height * (Math.Cos(angle + Math.PI) + 1) / 2 + ScaleValuesYShift
                    }
                };

                Canvas.SetTop(label, HalfSize - yRadius * Math.Cos(angle));
                Canvas.SetLeft(label, HalfSize - ScaleValuesRadius * Math.Sin(angle));

                GaugeShield.Children.Add(label);
            }
        }

        private void DrawScaleMinorGrid()
        {
            if (ScaleMinorGridLines == 0)
                return;

            double count = (ScaleMajorGridLines - 1) * (ScaleMinorGridLines + 1);
            double shift = (ScaleAngleStartRadians - ScaleAngleEndRadians) / count;
            double innerScaleRadius = OuterScaleRadius - ScaleMinorGridLineLength;
            for (int i = 0; i <= count; i++)
            {
                double angle = ScaleAngleStartRadians - i * shift;
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

        private void DrawIndicatorArrow()
        {
            double sinus = Math.Sin(ScaleAngleStartRadians);
            double cosine = Math.Cos(ScaleAngleStartRadians);

            Indicator = new Line
            {
                Stroke = IndicatorBrush,
                StrokeThickness = IndicatorThickness,
                X1 = HalfSize - IndicatorInnerRange * sinus,
                Y1 = HalfSize - IndicatorInnerRange * cosine,
                X2 = HalfSize - IndicatorOuterRange * sinus,
                Y2 = HalfSize - IndicatorOuterRange * cosine,
                StrokeEndLineCap = PenLineCap.Triangle,
                StrokeStartLineCap = PenLineCap.Square,
            };

            ValueChanged(this, new DependencyPropertyChangedEventArgs(ValueProperty, Value, Value));
            GaugeShield.Children.Add(Indicator);
        }

        private void DrawText()
        {
            var label = new TextBlock
            {
                Foreground = TextBrush,
                FontSize = TextSize,
                TextAlignment = TextAlignment.Center,
                Width = HalfSize,
                Text = Text,
                FontFamily = TextFont
            };

            Canvas.SetTop(label, HalfSize + TextYShift);
            Canvas.SetLeft(label, HalfSize / 2);

            GaugeShield.Children.Add(label);
        }
    }
}

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Common.WPF
{
    public static class ArcDrawer
    {
        public static void DrawSimpleArc(this Canvas canvas, Point startPoint, Point endPoint, Size size, Brush brush = default!, double thickness = 1, SweepDirection direction = SweepDirection.Clockwise)
        {
            var arcSeg = new ArcSegment
            {
                Point = endPoint,
                Size = size,
                IsLargeArc = true,
                SweepDirection = direction
            };
            var pthFigure = new PathFigure
            {
                StartPoint = startPoint,
                Segments = [arcSeg]
            };
            var arcPath = new Path
            {
                Stroke = brush,
                StrokeThickness = thickness,
                Data = new PathGeometry { Figures = [pthFigure] },
                StrokeEndLineCap = PenLineCap.Square,
                StrokeStartLineCap = PenLineCap.Square
            };
            canvas.Children.Add(arcPath);
        }
    }
}

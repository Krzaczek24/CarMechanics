using Common.Extensions;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace ControlPanel.Extensions
{
    internal static class VectorExtension
    {
        private const double multiplier = 0.90;

        public static Vector Normalize(this Vector vector, UserControl control)
        {
            if (vector == default)
            {
                return vector;
            }
            double length = Math.Min(vector.Length, 1.0);
            vector.Normalize();
            vector *= length;
            vector *= control.GetDimensionsMatrix();
            vector *= multiplier;
            return vector;
        }

        public static Matrix GetDimensionsMatrix(this UserControl control)
        {
            return new Matrix()
            {
                M11 = control.ActualWidth,
                M22 = control.ActualHeight
            };
        }

        public static double GetAngle(this Vector vector)
        {
            return Math.Atan2(vector.Y, vector.X);
        }

        public static Vector Squared(this Vector vector)
        {
            double angle = vector.GetAngle();
            var (sin, cos) = Math.SinCos(angle);
            (sin, cos) = (Math.Abs(sin), Math.Abs(cos));
            double factor = Math.Max(sin, cos);
            return vector / factor;
        }

        public static Vector CutOff(this Vector vector)
        {
            static double CutOff(double value) => value.CutOff(-1, 1);
            return new Vector()
            {
                X = CutOff(vector.X),
                Y = CutOff(vector.Y)
            };
        }

        public static Thickness AsThickness(this Vector vector)
        {
            return new Thickness()
            {
                Left = vector.X,
                Top = -vector.Y
            };
        }
    }
}

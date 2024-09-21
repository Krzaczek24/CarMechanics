using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace XboxPanel.Extensions
{
    internal static class VectorExtension
    {
        public static Vector Normalize(this Vector vector, UserControl control)
        {
            vector *= control.GetDimensionsMatrix();
            return vector *= 0.8;
        }

        public static Matrix GetDimensionsMatrix(this UserControl control) => new() { M11 = control.ActualWidth, M22 = control.ActualHeight };

        public static Thickness AsThickness(this Vector vector) => new() { Left = vector.X, Top = -vector.Y };
    }
}

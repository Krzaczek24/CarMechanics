using System;

namespace Common
{
    public class Angle
    {
        private double _degrees;
        private double _radians;

        public double Degrees
        {
            get => _degrees;
            set
            {
                _degrees = value;
                _radians = value * Math.PI / 180;
            }
        }

        public double Radians
        {
            get => _radians;
            set
            {
                _radians = value;
                _degrees = value * 180 / Math.PI;
            }
        }

        public Angle Normalized => new() { Degrees = _degrees % 360 };

        public static Angle operator +(Angle a, Angle b) => new()
        {
            Degrees = a.Degrees + b.Degrees,
        };

        public static Angle operator -(Angle a, Angle b) => new()
        {
            Degrees = a.Degrees - b.Degrees,
        };

        public static bool operator ==(Angle a, Angle b) => a.Degrees == b.Degrees;

        public static bool operator !=(Angle a, Angle b) => !(a == b);

        public override bool Equals(object? obj) => obj switch
        {
            null => false,
            Angle angle => Degrees == angle.Degrees,
            _ => false
        };

        public override int GetHashCode() => Degrees.GetHashCode();
    }
}

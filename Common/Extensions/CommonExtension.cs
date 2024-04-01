using System;
using System.Linq;
using System.Numerics;

namespace Common.Extensions
{
    public static class CommonExtension
    {
        public static bool IsIn<T>(this T source, params T[] collection)
        {
            return collection.Contains(source);
        }

        public static bool IsBetween<T>(this T value, T min, T max) where T : INumber<T>
        {
            return value >= min && value <= max;
        }

        public static T Scale<T>(this T value, T minSourceValue, T maxSourceValue, T minTargetValue, T maxTargetValue) where T : INumber<T>
        {
            return minTargetValue + (value - minSourceValue) / (maxSourceValue - minSourceValue) * (maxTargetValue - minTargetValue);
        }

        public static T CutOff<T>(this T value, T min, T max) where T : INumber<T>
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public static T ScaleWithCutOff<T>(this T value, T minSourceValue, T maxSourceValue, T minTargetValue, T maxTargetValue) where T : INumber<T>
        {
            return value.Scale(minSourceValue, maxSourceValue, minTargetValue, maxTargetValue).CutOff(minTargetValue, maxTargetValue);
        }

        public static T WithThreshold<T>(this T value, T threshold) where T : INumber<T>
        {
            return T.Abs(value) >= threshold ? value : T.Zero;
        }
    }
}

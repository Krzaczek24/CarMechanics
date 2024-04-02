using Common.Extensions;
using SharpDX.XInput;
using System.Windows;

namespace ControlPanel.Extensions
{
    internal static class GamepadExtension
    {
        public static Vector GetLeftStickValue(this Gamepad gamepad, double threshold = 0.0)
        {
            return GetKnobValue(gamepad.LeftThumbX, gamepad.LeftThumbY, threshold);
        }

        public static Vector GetRightStickValue(this Gamepad gamepad, double threshold = 0.0)
        {
            return GetKnobValue(gamepad.RightThumbX, gamepad.RightThumbY, threshold);
        }

        public static double GetLeftTriggerValue(this Gamepad gamepad)
        {
            return GetTriggerValue(gamepad.LeftTrigger);
        }

        public static double GetRightTriggerValue(this Gamepad gamepad)
        {
            return GetTriggerValue(gamepad.RightTrigger);
        }

        private static Vector GetKnobValue(short x, short y, double threshold = 0)
        {
            var vector = new Vector()
            {
                X = GetStickAxisValue(x),
                Y = GetStickAxisValue(y)
            };

            if (vector.Length <= threshold)
            {
                return new();
            }

            double length = (vector.Length - threshold).ScaleWithCutOff(0, 1 - threshold, 0, 1);
            vector.Normalize();
            vector *= length;
            return vector;
        }

        private static double GetStickAxisValue(short value)
        {
            return ((double)value).ScaleWithCutOff(short.MinValue, short.MaxValue, -1, 1);
        }

        private static double GetTriggerValue(byte value)
        {
            return ((double)value).Scale(byte.MinValue, byte.MaxValue, 0, 1);
        }
    }
}

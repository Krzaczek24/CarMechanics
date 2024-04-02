
using System.Windows;

namespace Common
{
    public record class ControllerDataDto
    {
        private const char delimiter = '|';

        public bool Connected { get; set; }
        public Vector LeftStick { get; set; }
        public Vector RightStick { get; set; }
        public double LeftTrigger { get; set; }
        public double RightTrigger { get; set; }
        public DPadDataDto DPad { get; set; } = new();
        public ButtonsDataDto Buttons { get; set; } = new();

        public override string ToString()
        {
            return string.Join(delimiter, [
                Converter.Convert(Connected),
                Converter.Convert(LeftStick),
                Converter.Convert(RightStick),
                Converter.Convert(LeftTrigger),
                Converter.Convert(RightTrigger),
                DPad.ToString(),
                Buttons.ToString(),
            ]);
        }

        public static string ToString(ControllerDataDto data) => data.ToString();

        public static ControllerDataDto FromString(string text)
        {
            var values = text.Split(delimiter);
            return new()
            {
                Connected = Converter.ConvertBool(values[0][0]),
                LeftStick = Converter.ConvertVector(values[1]),
                RightStick = Converter.ConvertVector(values[2]),
                LeftTrigger = Converter.ConvertDouble(values[3]),
                RightTrigger = Converter.ConvertDouble(values[4]),
                DPad = DPadDataDto.FromString(values[5]),
                Buttons = ButtonsDataDto.FromString(values[6]),
            };
        }
    }

    public record class DPadDataDto
    {
        public bool Up { get; set; }
        public bool Down { get; set; }
        public bool Left { get; set; }
        public bool Right { get; set; }

        public override string ToString()
        {
            return new string([
                Converter.Convert(Up),
                Converter.Convert(Down),
                Converter.Convert(Left),
                Converter.Convert(Right)
            ]);
        }

        public static string ToString(DPadDataDto data) => data.ToString();

        public static DPadDataDto FromString(string value)
        {
            return new()
            {
                Up = Converter.ConvertBool(value[0]),
                Down = Converter.ConvertBool(value[1]),
                Left = Converter.ConvertBool(value[2]),
                Right = Converter.ConvertBool(value[3]),
            };
        }
    }

    public record class ButtonsDataDto
    {
        public bool A { get; set; }
        public bool B { get; set; }
        public bool X { get; set; }
        public bool Y { get; set; }

        public bool Back { get; set; }
        public bool Start { get; set; }

        public bool LeftShoulder { get; set; }
        public bool RightShoulder { get; set; }

        public bool LeftThumb { get; set; }
        public bool RightThumb { get; set; }

        public override string ToString()
        {
            return new string([
                Converter.Convert(A),
                Converter.Convert(B),
                Converter.Convert(X),
                Converter.Convert(Y),
                Converter.Convert(Back),
                Converter.Convert(Start),
                Converter.Convert(LeftShoulder),
                Converter.Convert(RightShoulder),
                Converter.Convert(LeftThumb),
                Converter.Convert(RightThumb)
            ]);
        }

        public static string ToString(ButtonsDataDto data) => data.ToString();

        public static ButtonsDataDto FromString(string value)
        {
            return new()
            {
                A = Converter.ConvertBool(value[0]),
                B = Converter.ConvertBool(value[1]),
                X = Converter.ConvertBool(value[2]),
                Y = Converter.ConvertBool(value[3]),
                Back = Converter.ConvertBool(value[4]),
                Start = Converter.ConvertBool(value[5]),
                LeftShoulder = Converter.ConvertBool(value[6]),
                RightShoulder = Converter.ConvertBool(value[7]),
                LeftThumb = Converter.ConvertBool(value[8]),
                RightThumb = Converter.ConvertBool(value[9])
            };
        }
    }

    public static class Converter
    {
        private const char delimiter = '#';

        public static char Convert(bool value) => value ? '1' : '0';
        public static bool ConvertBool(char value) => value == '1';

        public static string Convert(double value, bool withSign = false) => value.ToString(withSign ? "+0.000;-0.000" : "0.000");
        public static double ConvertDouble(string value) => double.Parse(value);

        public static string Convert(Vector value) => Convert(value.X, true) + delimiter + Convert(value.Y, true);
        public static Vector ConvertVector(string value)
        {
            var values = value.Split(delimiter);
            return new Vector(ConvertDouble(values[0]), ConvertDouble(values[1]));
        }
    }
}

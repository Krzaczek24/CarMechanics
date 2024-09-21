using Common.Controller;
using System.Numerics;

namespace XboxPanel.Controller
{
    public static class ControllerDataExtension
    {
        public static ControllerDataDto ToDto(this ControllerData data)
        {
            return new ControllerDataDto()
            {
                ThumbLeft = Convert(data.ThumbLeft.Value),
                ThumbRight = Convert(data.ThumbRight.Value),
                TriggerLeft = data.TriggerLeft.Value,
                TriggerRight = data.TriggerRight.Value,
                Button = new Button()
                {
                    A = data.Button.A.Value,
                    B = data.Button.B.Value,
                    X = data.Button.X.Value,
                    Y = data.Button.Y.Value,
                    DPad = new DPad()
                    {
                        Up = data.Button.DPadUp.Value,
                        Down = data.Button.DPadDown.Value,
                        Left = data.Button.DPadLeft.Value,
                        Right = data.Button.DPadRight.Value
                    },
                    Start = data.Button.Start.Value,
                    Back = data.Button.Back.Value,
                    ShoulderLeft = data.Button.ShoulderLeft.Value,
                    ShoulderRight = data.Button.ShoulderRight.Value,
                    ThumbLeft = data.Button.ThumbLeft.Value,
                    ThumbRight = data.Button.ThumbRight.Value
                }
            };
        }

        private static Vector2 Convert(System.Windows.Vector vector) => new() { X = (float)vector.X, Y = (float)vector.Y };
    }
}

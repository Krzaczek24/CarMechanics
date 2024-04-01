
using System.Windows;

namespace Common
{
    public class ControllerDataDto
    {
        public bool Connected { get; set; }
        public Vector LeftStick { get; set; }
        public Vector RightStick { get; set; }
        public double LeftTrigger { get; set; }
        public double RightTrigger { get; set; }
        public DPadDataDto DPad { get; set; } = new();
        public ButtonsDataDto Buttons { get; set; } = new();
    }

    public class DPadDataDto
    {
        public bool Up { get; set; }
        public bool Down { get; set; }
        public bool Left { get; set; }
        public bool Right { get; set; }
    }

    public class ButtonsDataDto
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
    }
}

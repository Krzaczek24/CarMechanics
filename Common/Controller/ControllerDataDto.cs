using System.Numerics;

namespace Common.Controller
{
    public struct ControllerDataDto
    {
        public Vector2 ThumbLeft { get; set; }
        public Vector2 ThumbRight { get; set; }
        public double TriggerLeft { get; set; }
        public double TriggerRight { get; set; }
        public Button Button { get; set; }
    }

    public struct Button
    {
        public bool A { get; set; }
        public bool B { get; set; }
        public bool X { get; set; }
        public bool Y { get; set; }

        public DPad DPad { get; set; }
        public bool ShoulderLeft { get; set; }
        public bool ShoulderRight { get; set; }
        public bool ThumbLeft { get; set; }
        public bool ThumbRight { get; set; }

        public bool Back { get; set; }
        public bool Start { get; set; }
    }

    public struct DPad
    {
        public bool Up { get; set; }
        public bool Down { get; set; }
        public bool Left { get; set; }
        public bool Right { get; set; }
    } 
}

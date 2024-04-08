using System;

namespace Common.Controller
{
    [Flags]
    public enum ControllerEventSource
    {
        None = 0,

        Connection = 1 << 0,

        LeftThumb = 1 << 1,
        RightThumb = 1 << 2,
        Thumbs = LeftThumb | RightThumb,

        LeftTrigger = 1 << 3,
        RightTrigger = 1 << 4,
        Triggers = LeftTrigger | RightTrigger,

        Analogs = Thumbs | Triggers,

        LeftThumbButton = 1 << 5,
        RightThumbButton = 1 << 6,
        ThumbButtons = LeftThumbButton | RightThumbButton,

        LeftShoulder = 1 << 7,
        RightShoulder = 1 << 8,
        Shoulders = LeftShoulder | RightShoulder,

        A = 1 << 9,
        B = 1 << 10,
        X = 1 << 11,
        Y = 1 << 12,
        Letters = A | B | X | Y,

        DPadUp = 1 << 13,
        DPadDown = 1 << 14,
        DPadLeft = 1 << 15,
        DPadRight = 1 << 16,
        DPad = DPadUp | DPadDown | DPadLeft | DPadRight,

        Back = 1 << 17,
        Start = 1 << 18,
        Functional = Back | Start,

        Buttons = ThumbButtons | Shoulders | Letters | DPad | Functional
    }
}

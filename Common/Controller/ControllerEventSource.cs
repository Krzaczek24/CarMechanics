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

        TriggerLeft = 1 << 3,
        TriggerRight = 1 << 4,
        Triggers = TriggerLeft | TriggerRight,

        Analogs = Thumbs | Triggers,

        ThumbLeftButton = 1 << 5,
        ThumbRightButton = 1 << 6,
        ThumbButtons = ThumbLeftButton | ThumbRightButton,

        ShoulderLeft = 1 << 7,
        ShoulderRight = 1 << 8,
        Shoulders = ShoulderLeft | ShoulderRight,

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

        Buttons = ThumbButtons | Shoulders | Letters | DPad | Functional,

        All = Analogs | Buttons
    }
}

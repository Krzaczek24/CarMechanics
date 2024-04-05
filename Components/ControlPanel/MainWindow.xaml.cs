using Common;
using ControlPanel.Extensions;
using SharpDX;
using SharpDX.XInput;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using UDP;

namespace ControlPanel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private volatile bool changed = false;
        private volatile bool changeHandled = true;
        private ControllerDataDto Data { get; set; } = new();

        public MainWindow()
        {
            InitializeComponent();
            GetPanelWriter().Run();
            GetControllerReader().Run();
            GetControllerDataSender().Run();
        }

        private Worker GetControllerDataSender()
        {
            var broadcaster = UdpBroadcaster.Create<ControllerDataDto>(10000, ControllerDataDto.ToString);
            return new Worker(() =>
            {
                if (changed || !changeHandled)
                {
                    broadcaster.SendAsync(Data);
                    changeHandled = true;
                }
            }, 100);
        }

        private UiWorker GetPanelWriter()
        {
            return new UiWorker(() =>
            {
                LeftStick.Vector = Data.LeftStick;
                RightStick.Vector = Data.RightStick;

                LeftTrigger.Value = Data.LeftTrigger;
                RightTrigger.Value = Data.RightTrigger;

                DPad.TopButton.Pressed = Data.DPad.Up;
                DPad.BottomButton.Pressed = Data.DPad.Down;
                DPad.LeftButton.Pressed = Data.DPad.Left;
                DPad.RightButton.Pressed = Data.DPad.Right;

                Letters.TopButton.Pressed = Data.Buttons.Y;
                Letters.BottomButton.Pressed = Data.Buttons.A;
                Letters.LeftButton.Pressed = Data.Buttons.X;
                Letters.RightButton.Pressed = Data.Buttons.B;

                LeftStickButton.Pressed = Data.Buttons.LeftThumb;
                RightStickButton.Pressed = Data.Buttons.RightThumb;

                LeftShoulderButton.Pressed = Data.Buttons.LeftShoulder;
                RightShoulderButton.Pressed = Data.Buttons.RightShoulder;

                BackButton.Pressed = Data.Buttons.Back;
                StartButton.Pressed = Data.Buttons.Start;
            }, Dispatcher, 10);
        }

        private Worker GetControllerReader()
        {
            var controller = new Controller(UserIndex.One);
            return new Worker(() =>
            {
                if (changed = Data.Connected != controller.IsConnected)
                {
                    Debug.WriteLine($"changed: {(changed ? 1 : 0)}");
                    changeHandled = false;
                }

                if (!(Data.Connected = controller.IsConnected))
                {
                    return;
                }

                try
                {
                    var gamepad = controller.GetState().Gamepad;

                    var data = new ControllerDataDto()
                    {
                        Connected = controller.IsConnected,

                        LeftStick = gamepad.GetLeftStickValue(0.15),
                        RightStick = gamepad.GetRightStickValue(0.10),

                        LeftTrigger = gamepad.GetLeftTriggerValue(),
                        RightTrigger = gamepad.GetRightTriggerValue(),

                        DPad = new DPadDataDto()
                        {
                            Up = gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadUp),
                            Down = gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadDown),
                            Left = gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadLeft),
                            Right = gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadRight),
                        },

                        Buttons = new ButtonsDataDto()
                        {
                            A = gamepad.Buttons.HasFlag(GamepadButtonFlags.A),
                            B = gamepad.Buttons.HasFlag(GamepadButtonFlags.B),
                            X = gamepad.Buttons.HasFlag(GamepadButtonFlags.X),
                            Y = gamepad.Buttons.HasFlag(GamepadButtonFlags.Y),

                            LeftThumb = gamepad.Buttons.HasFlag(GamepadButtonFlags.LeftThumb),
                            RightThumb = gamepad.Buttons.HasFlag(GamepadButtonFlags.RightThumb),

                            LeftShoulder = gamepad.Buttons.HasFlag(GamepadButtonFlags.LeftShoulder),
                            RightShoulder = gamepad.Buttons.HasFlag(GamepadButtonFlags.RightShoulder),

                            Back = gamepad.Buttons.HasFlag(GamepadButtonFlags.Back),
                            Start = gamepad.Buttons.HasFlag(GamepadButtonFlags.Start)
                        }
                    };

                    if (changed |= Data != data)
                    {
                        changeHandled = false;
                    }
                    Data = data;
                }
                catch (SharpDXException ex) when (ex.Message.Contains("The device is not connected", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (changed |= Data.Connected != false)
                    {
                        Debug.WriteLine($"changed 2: {(changed ? 1 : 0)}");
                        changeHandled = false;
                    }
                    Debug.WriteLine("not connected ex");
                    Data.Connected = false;
                }
            }, 10);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
    }
}
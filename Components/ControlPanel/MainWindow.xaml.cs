using Common;
using ControlPanel.Extensions;
using SharpDX;
using SharpDX.XInput;
using System.Windows;
using System.Windows.Input;

namespace ControlPanel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ControllerDataDto Data { get; } = new();

        public MainWindow()
        {
            InitializeComponent();
            GetPanelWriter().Run();
            GetControllerReader().Run();
            GetControllerDataSender().Run();
        }

        private Worker GetControllerDataSender()
        {
            return new Worker(() =>
            {

            }, 10);
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
                Data.Connected = controller.IsConnected;
                if (!Data.Connected)
                {
                    return;
                }

                try
                {
                    var data = controller.GetState().Gamepad;

                    Data.LeftStick = data.GetLeftStickValue(0.15);
                    Data.RightStick = data.GetRightStickValue(0.10);

                    Data.LeftTrigger = data.LeftTrigger;
                    Data.RightTrigger = data.RightTrigger;

                    Data.DPad.Up = data.Buttons.HasFlag(GamepadButtonFlags.DPadUp);
                    Data.DPad.Down = data.Buttons.HasFlag(GamepadButtonFlags.DPadDown);
                    Data.DPad.Left = data.Buttons.HasFlag(GamepadButtonFlags.DPadLeft);
                    Data.DPad.Right = data.Buttons.HasFlag(GamepadButtonFlags.DPadRight);

                    Data.Buttons.A = data.Buttons.HasFlag(GamepadButtonFlags.A);
                    Data.Buttons.B = data.Buttons.HasFlag(GamepadButtonFlags.B);
                    Data.Buttons.X = data.Buttons.HasFlag(GamepadButtonFlags.X);
                    Data.Buttons.Y = data.Buttons.HasFlag(GamepadButtonFlags.Y);

                    Data.Buttons.LeftThumb = data.Buttons.HasFlag(GamepadButtonFlags.LeftThumb);
                    Data.Buttons.RightThumb = data.Buttons.HasFlag(GamepadButtonFlags.RightThumb);

                    Data.Buttons.LeftShoulder = data.Buttons.HasFlag(GamepadButtonFlags.LeftShoulder);
                    Data.Buttons.RightShoulder = data.Buttons.HasFlag(GamepadButtonFlags.RightShoulder);

                    Data.Buttons.Back = data.Buttons.HasFlag(GamepadButtonFlags.Back);
                    Data.Buttons.Start = data.Buttons.HasFlag(GamepadButtonFlags.Start);
                }
                catch (SharpDXException ex) when (ex.Message.Contains("The device is not connected", StringComparison.InvariantCultureIgnoreCase)) { }
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
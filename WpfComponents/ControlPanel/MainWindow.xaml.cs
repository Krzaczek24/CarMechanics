using Common;
using Common.Controller;
using SharpDX;
using SharpDX.XInput;
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
        private static readonly ControllerOptions options = new()
        {
            Threshold = new()
            {
                Thumb = new()
                {
                    Left = 0.15,
                    Right = 0.10
                }
            }
        };

        private ControllerData Data { get; } = new(options);

        public MainWindow()
        {
            InitializeComponent();

            Data.Thumb.Left.OnValueChanged += value => Dispatcher.Invoke(() => LeftStick.Vector = value);
            Data.Thumb.Right.OnValueChanged += value => Dispatcher.Invoke(() => RightStick.Vector = value);
            Data.Trigger.Left.OnValueChanged += value => Dispatcher.Invoke(() => LeftTrigger.Value = value);
            Data.Trigger.Right.OnValueChanged += value => Dispatcher.Invoke(() => RightTrigger.Value = value);
            Data.Button.DPadUp.OnValueChanged += value => Dispatcher.Invoke(() => DPad.TopButton.Pressed = value); ;
            Data.Button.DPadDown.OnValueChanged += value => Dispatcher.Invoke(() => DPad.BottomButton.Pressed = value);
            Data.Button.DPadLeft.OnValueChanged += value => Dispatcher.Invoke(() => DPad.LeftButton.Pressed = value);
            Data.Button.DPadRight.OnValueChanged += value => Dispatcher.Invoke(() => DPad.RightButton.Pressed = value);
            Data.Button.Y.OnValueChanged += value => Dispatcher.Invoke(() => Letters.TopButton.Pressed = value);
            Data.Button.A.OnValueChanged += value => Dispatcher.Invoke(() => Letters.BottomButton.Pressed = value);
            Data.Button.X.OnValueChanged += value => Dispatcher.Invoke(() => Letters.LeftButton.Pressed = value);
            Data.Button.B.OnValueChanged += value => Dispatcher.Invoke(() => Letters.RightButton.Pressed = value);
            Data.Button.LeftThumb.OnValueChanged += value => Dispatcher.Invoke(() => LeftStickButton.Pressed = value);
            Data.Button.RightThumb.OnValueChanged += value => Dispatcher.Invoke(() => RightStickButton.Pressed = value);
            Data.Button.LeftShoulder.OnValueChanged += value => Dispatcher.Invoke(() => LeftShoulderButton.Pressed = value);
            Data.Button.RightShoulder.OnValueChanged += value => Dispatcher.Invoke(() => RightShoulderButton.Pressed = value);
            Data.Button.Back.OnValueChanged += value => Dispatcher.Invoke(() => BackButton.Pressed = value);
            Data.Button.Start.OnValueChanged += value => Dispatcher.Invoke(() => StartButton.Pressed = value);

            var broadcaster = UdpBroadcaster.Create<(ControllerEventSource, object)>(10000, ControllerDataSerializer.SerializeEvent);
            Data.OnStateChange += (ControllerEventSource source, object value) => broadcaster.SendAsync((source, value));

            GetControllerReader().Run();
        }

        private Worker GetControllerReader()
        {
            var controller = new Controller(UserIndex.One);
            return new Worker(() =>
            {
                if (!(Data.Connected.Value = controller.IsConnected))
                    return;

                Gamepad gamepad;
                try { gamepad = controller.GetState().Gamepad; }
                catch (SharpDXException ex) when (ex.Message.Contains("The device is not connected", StringComparison.InvariantCultureIgnoreCase))
                {
                    Data.Connected.Value = false;
                    return;
                }

                Data.Thumb.Left.RawValue = (gamepad.LeftThumbX, gamepad.LeftThumbY);
                Data.Thumb.Right.RawValue = (gamepad.RightThumbX, gamepad.RightThumbY);

                Data.Trigger.Left.RawValue = gamepad.LeftTrigger;
                Data.Trigger.Right.RawValue = gamepad.RightTrigger;

                Data.Button.DPadUp.Value = gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadUp);
                Data.Button.DPadDown.Value = gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadDown);
                Data.Button.DPadLeft.Value = gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadLeft);
                Data.Button.DPadRight.Value = gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadRight);

                Data.Button.A.Value = gamepad.Buttons.HasFlag(GamepadButtonFlags.A);
                Data.Button.B.Value = gamepad.Buttons.HasFlag(GamepadButtonFlags.B);
                Data.Button.X.Value = gamepad.Buttons.HasFlag(GamepadButtonFlags.X);
                Data.Button.Y.Value = gamepad.Buttons.HasFlag(GamepadButtonFlags.Y);

                Data.Button.LeftThumb.Value = gamepad.Buttons.HasFlag(GamepadButtonFlags.LeftThumb);
                Data.Button.RightThumb.Value = gamepad.Buttons.HasFlag(GamepadButtonFlags.RightThumb);

                Data.Button.LeftShoulder.Value = gamepad.Buttons.HasFlag(GamepadButtonFlags.LeftShoulder);
                Data.Button.RightShoulder.Value = gamepad.Buttons.HasFlag(GamepadButtonFlags.RightShoulder);

                Data.Button.Back.Value = gamepad.Buttons.HasFlag(GamepadButtonFlags.Back);
                Data.Button.Start.Value = gamepad.Buttons.HasFlag(GamepadButtonFlags.Start);
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
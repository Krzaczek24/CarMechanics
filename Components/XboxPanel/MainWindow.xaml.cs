using Common;
using Common.Controller;
using Common.Tools;
using SharpDX;
using SharpDX.XInput;
using System.Windows;
using System.Windows.Input;
using XboxPanel.Controller;
using static Common.Constants.Settings;

namespace ControlPanel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly ControllerOptions options = new() { Threshold = new() { Thumb = new() { Left = 0.15, Right = 0.10 } } };

        private ControllerData Data { get; } = new(options);

        public MainWindow()
        {
            InitializeComponent();
            InitializeXboxPanel();
            InitializeDataTransfer();
            GetControllerReader().Run();
        }

        private void InitializeDataTransfer()
        {
            var writer = new MemorySharedFileWriter<ControllerDataDto>(MemorySharedFile.TEMP_FILE_PATH, MemorySharedFile.CONTROLLER_DATA_MAP);
            Data.OnStateChange += (_, _) => writer.Write(Data.ToDto());
        }

        private void InitializeXboxPanel()
        {
            Data.ThumbLeft.OnValueChanged += value => Dispatcher.Invoke(() => LeftStick.Vector = value);
            Data.ThumbRight.OnValueChanged += value => Dispatcher.Invoke(() => RightStick.Vector = value);
            Data.TriggerLeft.OnValueChanged += value => Dispatcher.Invoke(() => LeftTrigger.Value = value);
            Data.TriggerRight.OnValueChanged += value => Dispatcher.Invoke(() => RightTrigger.Value = value);
            Data.Button.DPadUp.OnValueChanged += value => Dispatcher.Invoke(() => DPad.TopButton.Pressed = value);
            Data.Button.DPadDown.OnValueChanged += value => Dispatcher.Invoke(() => DPad.BottomButton.Pressed = value);
            Data.Button.DPadLeft.OnValueChanged += value => Dispatcher.Invoke(() => DPad.LeftButton.Pressed = value);
            Data.Button.DPadRight.OnValueChanged += value => Dispatcher.Invoke(() => DPad.RightButton.Pressed = value);
            Data.Button.Y.OnValueChanged += value => Dispatcher.Invoke(() => Letters.TopButton.Pressed = value);
            Data.Button.A.OnValueChanged += value => Dispatcher.Invoke(() => Letters.BottomButton.Pressed = value);
            Data.Button.X.OnValueChanged += value => Dispatcher.Invoke(() => Letters.LeftButton.Pressed = value);
            Data.Button.B.OnValueChanged += value => Dispatcher.Invoke(() => Letters.RightButton.Pressed = value);
            Data.Button.ThumbLeft.OnValueChanged += value => Dispatcher.Invoke(() => LeftStickButton.Pressed = value);
            Data.Button.ThumbRight.OnValueChanged += value => Dispatcher.Invoke(() => RightStickButton.Pressed = value);
            Data.Button.ShoulderLeft.OnValueChanged += value => Dispatcher.Invoke(() => LeftShoulderButton.Pressed = value);
            Data.Button.ShoulderRight.OnValueChanged += value => Dispatcher.Invoke(() => RightShoulderButton.Pressed = value);
            Data.Button.Back.OnValueChanged += value => Dispatcher.Invoke(() => BackButton.Pressed = value);
            Data.Button.Start.OnValueChanged += value => Dispatcher.Invoke(() => StartButton.Pressed = value);
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

                Data.ThumbLeft.RawValue = (gamepad.LeftThumbX, gamepad.LeftThumbY);
                Data.ThumbRight.RawValue = (gamepad.RightThumbX, gamepad.RightThumbY);

                Data.TriggerLeft.RawValue = gamepad.LeftTrigger;
                Data.TriggerRight.RawValue = gamepad.RightTrigger;

                Data.Button.DPadUp.Value = gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadUp);
                Data.Button.DPadDown.Value = gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadDown);
                Data.Button.DPadLeft.Value = gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadLeft);
                Data.Button.DPadRight.Value = gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadRight);

                Data.Button.A.Value = gamepad.Buttons.HasFlag(GamepadButtonFlags.A);
                Data.Button.B.Value = gamepad.Buttons.HasFlag(GamepadButtonFlags.B);
                Data.Button.X.Value = gamepad.Buttons.HasFlag(GamepadButtonFlags.X);
                Data.Button.Y.Value = gamepad.Buttons.HasFlag(GamepadButtonFlags.Y);

                Data.Button.ThumbLeft.Value = gamepad.Buttons.HasFlag(GamepadButtonFlags.LeftThumb);
                Data.Button.ThumbRight.Value = gamepad.Buttons.HasFlag(GamepadButtonFlags.RightThumb);

                Data.Button.ShoulderLeft.Value = gamepad.Buttons.HasFlag(GamepadButtonFlags.LeftShoulder);
                Data.Button.ShoulderRight.Value = gamepad.Buttons.HasFlag(GamepadButtonFlags.RightShoulder);

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
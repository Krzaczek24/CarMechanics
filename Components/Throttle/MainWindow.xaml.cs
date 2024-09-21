using Common;
using Common.Constants;
using Common.Controller;
using Common.Tools;
using KrzaqTools.Extensions;
using System.Windows;
using System.Windows.Input;
using Throttle.Constants;

namespace Throttle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ValueObservable<byte> throttleAngle = new(ThrottleAngles.MINIMAL);

        public MainWindow()
        {
            InitializeComponent();
            InitializePanel();
#if DEBUG
            Thread.Sleep(1000);
#endif
            GetControllerDataReader().Run();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void InitializePanel()
        {
            throttleAngle.OnValueChanged += value => Dispatcher.Invoke(() =>
            {
                byte percent = (byte)((double)value).ScaleAndConvert(ThrottleAngles.MINIMAL, ThrottleAngles.MAXIMAL, 0, 100);
                ThrottlePercentTextBox.Text = $"{percent}%";
                ThrottleAngleTextBox.Text = $"{value}°";
                Throttle.Angle = throttleAngle.Value;
            });
        }

        private Worker GetControllerDataReader()
        {
            var reader = new MemorySharedFileReader<ControllerDataDto>(Settings.MemorySharedFile.TEMP_FILE_PATH, Settings.MemorySharedFile.CONTROLLER_DATA_MAP);
            return new Worker(() =>
            {
                var controllerData = reader.Read();
                byte angle = (byte)(controllerData.TriggerRight * 90);
                throttleAngle.Value = Math.Max(angle, ThrottleAngles.MINIMAL);
            }, 10);
        }
    }
}
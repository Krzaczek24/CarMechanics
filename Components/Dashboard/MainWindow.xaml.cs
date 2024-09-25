using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Common.WPF;
using KrzaqTools.Extensions;

namespace Dashboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Lazy<IEnumerable<Image>> Indicators { get; }

        public MainWindow()
        {
            Indicators = new(GetIndicators);

            InitializeComponent();
            InitializeIndicators();
            InitializeUIWorkers();
            InitializeDataReceivers();
        }

        private void InitializeIndicators() => Indicators.Value.ForEach(img => img.Opacity = 0);

        private void InitializeUIWorkers()
        {
            new UiWorker(() =>
            {
                var now = DateTime.Now;
                Clock.Value = now.Hour * 60 + now.Minute;
            }, Dispatcher, 60000).Run();
        }

        private void InitializeDataReceivers()
        {

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private IEnumerable<Image> GetIndicators()
        {
            var fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            var indicators = fields.Where(x => x.FieldType == typeof(Image) && x.Name.StartsWith("Indicator"));
            var images = indicators.Select(x => (Image)x.GetValue(this)!);
            return images;
        }
    }
}
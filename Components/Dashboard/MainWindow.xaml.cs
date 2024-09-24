using System.Windows;
using System.Windows.Input;

namespace Dashboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeUIWorkers();
            InitializeDataReceivers();
        }

        private void InitializeUIWorkers()
        {

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
    }
}
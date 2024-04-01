using System;

namespace Clutch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private Logic Logic { get; } = new();

        public MainWindow()
        {
            InitializeComponent();

            Logic.FrictionCoefficient = FrictionSlider.Value;
            Logic.PlateOutterDiameter = int.Parse(PlateOutterDiameter.Text);
            Logic.PlateInnerDiameter = int.Parse(PlateInnerDiameter.Text);
            Logic.AxialForce = int.Parse(AxialForce.Text);
            Logic.PlateAngle = PlateAngleSlider.Value;

            RefreshMaxValues();
        }

        private void FrictionSlider_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            double value = Math.Round(FrictionSlider.Value, 2);
            Logic.FrictionCoefficient = value;
            FrictionTextBox.Text = value.ToString();
            RefreshMaxValues();
        }

        private void PlateOutterDiameter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (int.TryParse(PlateOutterDiameter.Text, out int value))
            {
                Logic.PlateOutterDiameter = value;
                RefreshMaxValues();
            }
        }

        private void PlateInnerDiameter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (int.TryParse(PlateInnerDiameter.Text, out int value))
            {
                Logic.PlateInnerDiameter = value;
                RefreshMaxValues();
            }
        }

        private void AxialForce_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (int.TryParse(AxialForce.Text, out int value))
            {
                Logic.AxialForce = value;
                RefreshMaxValues();
            }
        }

        private void PlateAngleSlider_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            int value = (int)PlateAngleSlider.Value;
            Logic.PlateAngle = value;
            PlateAngleTextBox.Text = value.ToString();
            RefreshMaxValues();
        }

        private void InputRPMsSlider_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            int value = (int)InputRPMsSlider.Value;
            Logic.RPM = value;
            InputRPMsTextBox.Text = value.ToString();

            if (MaxPower != null)
            {
                MaxPower.Text = Logic.MaxPower.ToString("F0");
            }
        }

        private void RefreshMaxValues()
        {
            if (MaxTorque != null && MaxPower != null)
            {
                MaxTorque.Text = Logic.MaxTorque.ToString("F0");
                MaxPower.Text = Logic.MaxPower.ToString("F0");
            }
        }
    }
}
